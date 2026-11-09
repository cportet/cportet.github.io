using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;
using Markdig;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace MyHomePage.Content;

/// <summary>
/// Loads the site content (wwwroot/content) once at startup, and exposes it with
/// localization and live values ({{age}}, {{experienceYears}}…) applied.
/// </summary>
public sealed class ContentService(HttpClient http, LanguageService language)
{
    private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder()
        .UseEmphasisExtras()
        .UseAutoLinks()
        .Build();

    private readonly Dictionary<string, Task<string?>> _markdownCache = [];

    public SiteData Site { get; private set; } = null!;
    public Profile Profile { get; private set; } = null!;
    public IReadOnlyList<Experience> Experiences { get; private set; } = [];
    public ResumeData Resume { get; private set; } = null!;
    public PersoData Perso { get; private set; } = null!;
    public ProjectsData Projects { get; private set; } = null!;
    public AboutData About { get; private set; } = null!;
    public AppConfig Config { get; private set; } = null!;

    private Dictionary<string, PageMeta> _pages = [];
    private Dictionary<string, LocalizedText> _ui = [];

    public string Language => language.Current;

    public async Task LoadAsync()
    {
        var site = Load("content/site.json", ContentJsonContext.Default.SiteData);
        var profile = Load("content/profile.json", ContentJsonContext.Default.Profile);
        var experiences = Load("content/resume/experiences.json", ContentJsonContext.Default.IReadOnlyListExperience);
        var resume = Load("content/resume/resume.json", ContentJsonContext.Default.ResumeData);
        var perso = Load("content/perso.json", ContentJsonContext.Default.PersoData);
        var projects = Load("content/projects.json", ContentJsonContext.Default.ProjectsData);
        var about = Load("content/about.json", ContentJsonContext.Default.AboutData);
        var pages = Load("content/pages.json", ContentJsonContext.Default.DictionaryStringPageMeta);
        var ui = Load("content/ui.json", ContentJsonContext.Default.DictionaryStringLocalizedText);
        var config = Load("appconfig.json", ContentJsonContext.Default.AppConfig);

        await Task.WhenAll(site, profile, experiences, resume, perso, projects, about, pages, ui, config);

        Site = site.Result;
        Profile = profile.Result;
        Experiences = experiences.Result.OrderByDescending(e => e.Start).ToList();
        Resume = resume.Result;
        Perso = perso.Result;
        Projects = projects.Result;
        About = about.Result;
        _pages = pages.Result;
        _ui = ui.Result;
        Config = config.Result;
    }

    // --- Localization ----------------------------------------------------------------

    /// <summary>Text in the current language, with live values expanded.</summary>
    public string T(LocalizedText? text) =>
        text is null ? string.Empty : Expand(text.Get(Language));

    /// <summary>Interface string from ui.json.</summary>
    public string Ui(string key) =>
        _ui.TryGetValue(key, out var text) ? T(text) : $"[{key}]";

    public PageMeta Page(string key) =>
        _pages.TryGetValue(key, out var meta)
            ? meta
            : throw new KeyNotFoundException($"Page '{key}' is missing from pages.json.");

    // --- Live values -----------------------------------------------------------------

    public static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

    public Experience? CurrentPosition => Experiences.FirstOrDefault(e => e.IsCurrent);

    public int Age => LiveDates.YearsBetween(Profile.BirthDate, Today);

    public int ExperienceYears => LiveDates.YearsBetween(Profile.CareerStart, Today);

    public string Duration(Experience experience) =>
        LiveDates.Duration(experience.Start, experience.End, Today, Language);

    public string Period(Experience experience) =>
        LiveDates.Period(experience.Start, experience.End, Language, Ui("date.present"));

    public string PeriodYears(Experience experience) =>
        LiveDates.PeriodYears(experience.Start, experience.End, Ui("date.present"));

    public string Expand(string text)
    {
        if (!text.Contains("{{", StringComparison.Ordinal))
            return text;

        return text
            .Replace("{{age}}", Age.ToString(), StringComparison.Ordinal)
            .Replace("{{experienceYears}}", ExperienceYears.ToString(), StringComparison.Ordinal)
            .Replace("{{currentRoleDuration}}", CurrentPosition is { } current ? Duration(current) : string.Empty, StringComparison.Ordinal)
            .Replace("{{year}}", Today.Year.ToString(), StringComparison.Ordinal);
    }

    // --- Markdown --------------------------------------------------------------------

    /// <summary>Renders content/md/{path}.{language}.md (falls back to French).</summary>
    public async Task<string?> MarkdownAsync(string path)
    {
        var key = $"{path}.{Language}";
        if (!_markdownCache.TryGetValue(key, out var task))
            _markdownCache[key] = task = LoadMarkdownAsync(path, Language);

        var markdown = await task;
        return markdown is null ? null : Markdig.Markdown.ToHtml(Expand(markdown), MarkdownPipeline);
    }

    private async Task<string?> LoadMarkdownAsync(string path, string lang)
    {
        string[] candidates = lang == Languages.Default ? [lang] : [lang, Languages.Default];
        foreach (var candidate in candidates)
        {
            using var request = NoCacheRequest($"content/md/{path}.{candidate}.md");
            using var response = await http.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
        }
        return null;
    }

    // --- Loading ---------------------------------------------------------------------

    private async Task<T> Load<T>(string path, JsonTypeInfo<T> typeInfo)
    {
        using var request = NoCacheRequest(path);
        using var response = await http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync(typeInfo)
            ?? throw new InvalidOperationException($"Content file '{path}' is empty.");
    }

    /// <summary>Revalidates with the server (ETag), so content updates show up without a stale browser cache.</summary>
    private static HttpRequestMessage NoCacheRequest(string path)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.SetBrowserRequestCache(BrowserRequestCache.NoCache);
        return request;
    }
}
