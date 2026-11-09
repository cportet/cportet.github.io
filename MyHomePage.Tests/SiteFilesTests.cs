using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace MyHomePage.Tests;

/// <summary>Keeps sitemap.xml and the manifest shortcuts in line with the routes of the app.</summary>
public class SiteFilesTests
{
    private const string BaseUrl = "https://www.portet.org";

    private static readonly string WwwRoot = Path.Combine(FindAppRoot(), "wwwroot");

    /// <summary>Public pages that search engines should know, in both languages.</summary>
    private static readonly Type[] IndexedPages =
    [
        typeof(Constants.Pages.Home),
        typeof(Constants.Pages.Resume),
        typeof(Constants.Pages.Projects),
        typeof(Constants.Pages.Privacy),
        typeof(Constants.Pages.Perso),
        typeof(Constants.Pages.Contact),
        typeof(Constants.Pages.About),
    ];

    [Fact]
    public void Sitemap_lists_every_public_page_in_both_languages()
    {
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var locations = XDocument.Load(Path.Combine(WwwRoot, "sitemap.xml"))
            .Descendants(ns + "loc")
            .Select(e => e.Value)
            .ToHashSet();

        var missing = IndexedPages
            .SelectMany(page => new[] { Route(page, "Route"), Route(page, "RouteEn") })
            .Select(route => BaseUrl + route)
            .Where(url => !locations.Contains(url))
            .ToList();

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

    private static string Route(Type page, string name) =>
        (string)page.GetField(name)!.GetRawConstantValue()!;

    private static string FindAppRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "MyHomePage.sln")))
            directory = directory.Parent;

        return Path.Combine(directory!.FullName, "MyHomePage");
    }
}
