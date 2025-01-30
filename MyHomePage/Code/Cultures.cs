using System.Globalization;

namespace MyHomePage.Code;

public static class Cultures
{
    public static readonly string[] SupportedLanguages = ["en", "fr"];

    public static void ApplyCulture(string culture)
    {
        var cultureInfo = new CultureInfo(culture);
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }

    public static List<CultureItem> GetCultures() =>
        SupportedLanguages
            .Select(code => new CultureItem(code, new CultureInfo(code).NativeName))
            .ToList();
}

public sealed record CultureItem(string Code, string Name);
