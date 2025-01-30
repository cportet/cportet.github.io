using System.Globalization;

namespace MyHomePage.Code;

public static class Cultures
{
    public static readonly string[] SupportedLanguages = ["en", "fr"];

    public static void ApplyCulture(string culture)
    {
        Console.WriteLine($"Culture: {culture}");

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
