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
        var site = Load(Constants.Files.Content.Site, ContentJsonContext.Default.SiteData);
        var profile = Load(Constants.Files.Content.Profile, ContentJsonContext.Default.Profile);
        var experiences = Load(Constants.Files.Content.Experiences, ContentJsonContext.Default.IReadOnlyListExperience);
        var resume = Load(Constants.Files.Content.Resume, ContentJsonContext.Default.ResumeData);
        var perso = Load(Constants.Files.Content.Perso, ContentJsonContext.Default.PersoData);
        var projects = Load(Constants.Files.Content.Projects, ContentJsonContext.Default.ProjectsData);
        var about = Load(Constants.Files.Content.About, ContentJsonContext.Default.AboutData);
        var pages = Load(Constants.Files.Content.Pages, ContentJsonContext.Default.DictionaryStringPageMeta);
        var ui = Load(Constants.Files.Content.Ui, ContentJsonContext.Default.DictionaryStringLocalizedText);
        var config = Load(Constants.Files.AppConfig, ContentJsonContext.Default.AppConfig);

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

    /// <summary>Email link with a subject in the current language.</summary>
    public string MailTo => $"mailto:{Site.Email}?subject={Uri.EscapeDataString(Ui(Constants.Ui.Contact.Email.Subject))}";

    public PageMeta Page(string key) =>
        _pages.TryGetValue(key, out var meta)
            ? meta
            : throw new KeyNotFoundException($"Page '{key}' is missing from pages.json.");

    // --- Live values -----------------------------------------------------------------

    public static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

    public Experience? CurrentPosition => Experiences.FirstOrDefault(e => e.IsCurrent);

    public int Age => LiveDates.YearsBetween(Profile.BirthDate, Today);

    public int ExperienceYears => LiveDates.YearsBetween(Profile.CareerStart, Today);

    public int ManagementYears => LiveDates.YearsBetween(Profile.ManagementStart, Today);

    public string Duration(Experience experience) =>
        LiveDates.Duration(experience.Start, experience.End, Today, Language);

    public string Period(Experience experience) =>
        LiveDates.Period(experience.Start, experience.End, Language, Ui(Constants.Ui.Date.Present));

    public string PeriodYears(Experience experience) =>
        LiveDates.PeriodYears(experience.Start, experience.End, Ui(Constants.Ui.Date.Present));

    public string Expand(string text)
    {
        if (!text.Contains("{{", StringComparison.Ordinal)) // Every token starts with "{{".
            return text;

        return text
            .Replace(Constants.Tokens.Age, Age.ToString(), StringComparison.Ordinal)
            .Replace(Constants.Tokens.ExperienceYears, ExperienceYears.ToString(), StringComparison.Ordinal)
            .Replace(Constants.Tokens.ManagementYears, ManagementYears.ToString(), StringComparison.Ordinal)
            .Replace(Constants.Tokens.CurrentRoleDuration, CurrentPosition is { } current ? Duration(current) : string.Empty, StringComparison.Ordinal)
            .Replace(Constants.Tokens.Year, Today.Year.ToString(), StringComparison.Ordinal);
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
            using var request = NoCacheRequest(string.Format(Constants.Files.Content.MarkdownFormat, path, candidate));
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
