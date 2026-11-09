using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MyHomePage.Content;

/// <summary>
/// The language comes from the URL: "/en/…" is English, everything else is French.
/// </summary>
public sealed class LanguageService : IDisposable
{
    private const string EnglishPrefix = Languages.English;
    private readonly NavigationManager _navigation;

    public LanguageService(NavigationManager navigation)
    {
        _navigation = navigation;
        _navigation.LocationChanged += OnLocationChanged;
        Current = FromUri(_navigation.Uri);
        ApplyCulture();
    }

    public string Current { get; private set; }

    public bool IsEnglish => Current == Languages.English;

    public CultureInfo Culture => Languages.Culture(Current);

    public event Action? Changed;

    /// <summary>Relative path of the current page, without the language prefix (e.g. "resume").</summary>
    public string CurrentPath => StripLanguage(_navigation.ToBaseRelativePath(_navigation.Uri));

    /// <summary>Builds a link to <paramref name="path"/> in the current (or given) language.</summary>
    public string Href(string path, string? language = null)
    {
        var target = (language ?? Current) == Languages.English ? EnglishPrefix : null;
        var clean = path.Trim('/');

        return (target, clean) switch
        {
            (null, "") => Constants.Pages.Home.Route,
            (null, _) => $"/{clean}",
            (_, "") => $"/{target}",
            _ => $"/{target}/{clean}"
        };
    }

    /// <summary>Same page, in another language.</summary>
    public string Switch(string language) => Href(CurrentPath, language);

    public static string FromUri(string uri)
    {
        var path = new Uri(uri).AbsolutePath.Trim('/');
        return path == EnglishPrefix || path.StartsWith(EnglishPrefix + "/", StringComparison.OrdinalIgnoreCase)
            ? Languages.English
            : Languages.French;
    }

    private static string StripLanguage(string relativePath)
    {
        var path = relativePath.Split('?', '#')[0].Trim('/');
        if (path == EnglishPrefix)
            return string.Empty;
        return path.StartsWith(EnglishPrefix + "/", StringComparison.OrdinalIgnoreCase)
            ? path[(EnglishPrefix.Length + 1)..]
            : path;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        var language = FromUri(e.Location);
        if (language == Current)
            return;

        Current = language;
        ApplyCulture();
        Changed?.Invoke();
    }

    private void ApplyCulture()
    {
        CultureInfo.DefaultThreadCurrentCulture = Culture;
        CultureInfo.DefaultThreadCurrentUICulture = Culture;
        CultureInfo.CurrentCulture = Culture;
        CultureInfo.CurrentUICulture = Culture;
    }

    public void Dispose() => _navigation.LocationChanged -= OnLocationChanged;
}
