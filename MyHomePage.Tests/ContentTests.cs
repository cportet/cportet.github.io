using Xunit;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using MyHomePage.Content;

namespace MyHomePage.Tests;

/// <summary>
/// Validates wwwroot/content: every file must deserialize, every text must exist in
/// every language, and every key used by the pages must exist.
/// </summary>
public partial class ContentTests
{
    private static readonly string AppRoot = FindAppRoot();
    private static readonly string ContentRoot = Path.Combine(AppRoot, "wwwroot", "content");

    private static readonly JsonDocumentOptions JsonOptions = new()
    {
        CommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public static TheoryData<string> JsonFiles() =>
        new(Directory.GetFiles(ContentRoot, "*.json", SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(ContentRoot, f)));

    [Fact]
    public void Every_content_file_deserializes()
    {
        var context = ContentJsonContext.Default;

        Assert.NotNull(Read("site.json", context.SiteData));
        Assert.NotNull(Read("profile.json", context.Profile));
        Assert.NotEmpty(Read("resume/experiences.json", context.IReadOnlyListExperience)!);
        Assert.NotNull(Read("resume/resume.json", context.ResumeData));
        Assert.NotNull(Read("perso.json", context.PersoData));
        Assert.NotNull(Read("projects.json", context.ProjectsData));
        Assert.NotNull(Read("about.json", context.AboutData));
        Assert.NotEmpty(Read("pages.json", context.DictionaryStringPageMeta)!);
        Assert.NotEmpty(Read("ui.json", context.DictionaryStringLocalizedText)!);
    }

    [Theory]
    [MemberData(nameof(JsonFiles))]
    public void Every_localized_text_exists_in_every_language(string file)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(ContentRoot, file)), JsonOptions);
        var incomplete = new List<string>();
        Visit(document.RootElement, "$", incomplete);

        Assert.Empty(incomplete);
    }

    [Fact]
    public void Exactly_one_position_is_current()
    {
        var experiences = Read("resume/experiences.json", ContentJsonContext.Default.IReadOnlyListExperience)!;

        Assert.Single(experiences, e => e.IsCurrent);
    }

    [Fact]
    public void Every_ui_key_constant_exists_in_ui_json()
    {
        var ui = Read("ui.json", ContentJsonContext.Default.DictionaryStringLocalizedText)!;
        var missing = ConstantValues(typeof(Constants.Ui)).Where(key => !ui.ContainsKey(key)).ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void Every_ui_json_key_has_a_constant()
    {
        var ui = Read("ui.json", ContentJsonContext.Default.DictionaryStringLocalizedText)!;
        var keys = ConstantValues(typeof(Constants.Ui)).ToHashSet();
        var orphans = ui.Keys.Where(key => !keys.Contains(key)).ToList();

        Assert.Empty(orphans);
    }

    [Fact]
    public void Every_page_key_constant_exists_in_pages_json()
    {
        var pages = Read("pages.json", ContentJsonContext.Default.DictionaryStringPageMeta)!;
        var missing = ConstantFields(typeof(Constants.Pages))
            .Where(f => f.Name == nameof(Constants.Pages.Home.Key))
            .Select(f => (string)f.GetRawConstantValue()!)
            .Where(key => !pages.ContainsKey(key))
            .ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void Every_content_file_constant_exists()
    {
        var wwwroot = Path.Combine(AppRoot, "wwwroot");
        var missing = ConstantFields(typeof(Constants.Files))
            .Where(f => f.Name != nameof(Constants.Files.Content.MarkdownFormat) && f.DeclaringType != typeof(Constants.Files.Markdown))
            .Select(f => ((string)f.GetRawConstantValue()!).TrimStart('/'))
            .Where(f => !File.Exists(Path.Combine(wwwroot, f)))
            .ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void Every_markdown_constant_exists_in_every_language()
    {
        var missing = ConstantValues(typeof(Constants.Files.Markdown))
            .SelectMany(path => Languages.Supported.Select(language => $"{path}.{language}.md"))
            .Where(file => !File.Exists(Path.Combine(ContentRoot, "md", file)))
            .ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void Every_markdown_file_is_used()
    {
        var markdown = Path.Combine(ContentRoot, "md");
        var used = ConstantValues(typeof(Constants.Files.Markdown)).ToHashSet();
        var orphans = Directory.GetFiles(markdown, "*.md", SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(markdown, f).Replace(Path.DirectorySeparatorChar, '/'))
            .Where(f => !used.Contains(f[..f.LastIndexOf('.', f.Length - 4)]))
            .ToList();

        Assert.Empty(orphans);
    }

    [Fact]
    public void No_hard_coded_key_or_path_in_the_pages()
    {
        var offenders = RazorFiles()
            .SelectMany(file => HardCodedRegex().Matches(File.ReadAllText(file))
                .Select(m => $"{Path.GetFileName(file)}: {m.Value}"))
            .ToList();

        Assert.Empty(offenders);
    }

    [Fact]
    public void Every_markdown_text_exists_in_every_language()
    {
        var markdown = Path.Combine(ContentRoot, "md");
        var missing = Directory.GetFiles(markdown, "*.md", SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(markdown, f))
            .Select(f => f[..f.LastIndexOf('.', f.Length - 4)])
            .Distinct()
            .SelectMany(path => Languages.Supported
                .Select(language => $"{path}.{language}.md")
                .Where(f => !File.Exists(Path.Combine(markdown, f))))
            .ToList();

        Assert.Empty(missing);
    }

    private static void Visit(JsonElement element, string path, List<string> incomplete)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var names = element.EnumerateObject().Select(p => p.Name).ToList();
                if (names.Count > 0 && names.All(n => Languages.Supported.Contains(n)))
                {
                    var absent = Languages.Supported.Except(names).ToList();
                    if (absent.Count > 0)
                        incomplete.Add($"{path}: missing {string.Join(", ", absent)}");
                    return;
                }
                foreach (var property in element.EnumerateObject())
                    Visit(property.Value, $"{path}.{property.Name}", incomplete);
                break;

            case JsonValueKind.Array:
                var index = 0;
                foreach (var item in element.EnumerateArray())
                    Visit(item, $"{path}[{index++}]", incomplete);
                break;
        }
    }

    private static T? Read<T>(string file, System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> typeInfo) =>
        JsonSerializer.Deserialize(File.ReadAllText(Path.Combine(ContentRoot, file)), typeInfo);

    private static IEnumerable<string> RazorFiles() =>
        Directory.GetFiles(AppRoot, "*.razor", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
                     && !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"));

    private static IEnumerable<FieldInfo> ConstantFields(Type type) =>
        type.GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && f.FieldType == typeof(string))
            .Concat(type.GetNestedTypes().SelectMany(ConstantFields));

    private static IEnumerable<string> ConstantValues(Type type) =>
        ConstantFields(type).Select(f => (string)f.GetRawConstantValue()!).Distinct();

    private static string FindAppRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "MyHomePage.sln")))
            directory = directory.Parent;

        return directory is null
            ? throw new DirectoryNotFoundException("Repository root (MyHomePage.sln) not found.")
            : Path.Combine(directory.FullName, "MyHomePage");
    }

    /// <summary>A string literal given to Ui(...), Href(...), Page="…", Path="…" or @page.</summary>
    [GeneratedRegex("""(?:Ui|Href|Content\.Page)\("|\b(?:Page|Path)="[^@"]|@page\s""")]
    private static partial Regex HardCodedRegex();
}
