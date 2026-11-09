using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using MyHomePage.Content;

namespace MyHomePage.Tools;

/// <summary>The content of wwwroot/content, read from disk, with the same localization and live values as the app.</summary>
internal sealed class SiteContent
{
    private readonly string _wwwroot;
    private readonly Dictionary<string, PageMeta> _pages;
    private readonly Dictionary<string, LocalizedText> _ui;

    public SiteContent(string wwwroot, DateOnly today)
    {
        _wwwroot = wwwroot;
        Today = today;

        var context = ContentJsonContext.Default;
        Site = Read(Constants.Files.Content.Site, context.SiteData);
        Profile = Read(Constants.Files.Content.Profile, context.Profile);
        Experiences = Read(Constants.Files.Content.Experiences, context.IReadOnlyListExperience).OrderByDescending(e => e.Start).ToList();
        Resume = Read(Constants.Files.Content.Resume, context.ResumeData);
        Perso = Read(Constants.Files.Content.Perso, context.PersoData);
        Projects = Read(Constants.Files.Content.Projects, context.ProjectsData);
        _pages = Read(Constants.Files.Content.Pages, context.DictionaryStringPageMeta);
        _ui = Read(Constants.Files.Content.Ui, context.DictionaryStringLocalizedText);
    }

    public DateOnly Today { get; }
    public SiteData Site { get; }
    public Profile Profile { get; }
    public IReadOnlyList<Experience> Experiences { get; }
    public ResumeData Resume { get; }
    public PersoData Perso { get; }
    public ProjectsData Projects { get; }

    public string BaseUrl => Site.BaseUrl.TrimEnd('/');

    public string T(LocalizedText? text, string language) =>
        text is null ? string.Empty : LiveDates.Expand(text.Get(language), Profile, Experiences, language, Today);

    public string Ui(string key, string language) => T(_ui[key], language);

    public PageMeta Page(string key) => _pages[key];

    /// <summary>Absolute URL of a page in a language.</summary>
    public string Url(string path, string language)
    {
        var clean = path.Trim('/');
        var prefix = language == Languages.English ? Constants.Pages.EnglishPrefix : string.Empty;
        return clean.Length == 0 && prefix.Length == 0
            ? $"{BaseUrl}/"
            : $"{BaseUrl}{prefix}{(clean.Length == 0 ? string.Empty : "/" + clean)}";
    }

    public string Markdown(string path, string language)
    {
        var file = Path.Combine(_wwwroot, string.Format(Constants.Files.Content.MarkdownFormat, path, language));
        return LiveDates.Expand(File.ReadAllText(file).Trim().TrimStart('﻿'), Profile, Experiences, language, Today);
    }

    private T Read<T>(string relativePath, JsonTypeInfo<T> typeInfo) =>
        JsonSerializer.Deserialize(File.ReadAllText(Path.Combine(_wwwroot, relativePath)), typeInfo)
        ?? throw new InvalidOperationException($"{relativePath} is empty.");
}
