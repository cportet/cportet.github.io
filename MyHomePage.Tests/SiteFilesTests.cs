using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;
using MyHomePage.Content;
using Xunit;

namespace MyHomePage.Tests;

/// <summary>Keeps sitemap.xml and the manifest shortcuts in line with the routes of the app.</summary>
public class SiteFilesTests
{
    private const string BaseUrl = "https://www.portet.org";

    private static readonly string WwwRoot = Path.Combine(FindAppRoot(), "wwwroot");

    [Fact]
    public void Sitemap_lists_every_public_page_in_both_languages()
    {
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var locations = XDocument.Load(Path.Combine(WwwRoot, "sitemap.xml"))
            .Descendants(ns + "loc")
            .Select(e => e.Value)
            .ToHashSet();

        var missing = IndexedUrls().Where(url => !locations.Contains(url)).ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void Manifest_shortcuts_point_to_existing_routes()
    {
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(WwwRoot, "manifest.webmanifest")));
        var routes = typeof(Constants.Pages).GetNestedTypes()
            .SelectMany(page => page.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Where(f => f.IsLiteral && f.Name is "Route" or "RouteEn")
            .Select(f => (string)f.GetRawConstantValue()!)
            .ToHashSet();

        var unknown = manifest.RootElement.GetProperty("shortcuts").EnumerateArray()
            .Select(s => s.GetProperty("url").GetString()!)
            .Where(url => !routes.Contains(url))
            .ToList();

        Assert.Empty(unknown);
    }

    [Fact]
    public void Llms_txt_lists_every_public_page_in_both_languages()
    {
        var llms = File.ReadAllText(Path.Combine(WwwRoot, Constants.Files.Llms));
        var missing = IndexedUrls().Where(url => !llms.Contains($"]({url})", StringComparison.Ordinal)).ToList();

        Assert.Empty(missing);
    }

    [Fact]
    public void Llms_full_txt_contains_every_position_in_both_languages()
    {
        var full = File.ReadAllText(Path.Combine(WwwRoot, Constants.Files.LlmsFull));
        var content = new MyHomePage.Tools.SiteContent(WwwRoot, DateOnly.FromDateTime(DateTime.Today));
        var missing = content.Experiences
            .SelectMany(e => Languages.Supported.Select(language => $"### {content.T(e.Role, language)} — {e.Company}"))
            .Where(title => !full.Contains(title, StringComparison.Ordinal))
            .ToList();

        Assert.Empty(missing);
    }

    private static IEnumerable<string> IndexedUrls() =>
        Constants.Pages.Indexed.SelectMany(page => Languages.Supported.Select(language =>
            language == Languages.English
                ? BaseUrl + Constants.Pages.EnglishPrefix + (page.Path.Length == 0 ? "" : "/" + page.Path)
                : BaseUrl + "/" + page.Path));

    private static string FindAppRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "MyHomePage.sln")))
            directory = directory.Parent;

        return Path.Combine(directory!.FullName, "MyHomePage");
    }
}
