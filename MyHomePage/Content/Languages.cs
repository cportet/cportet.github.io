using System.Globalization;

namespace MyHomePage.Content;

public static class Languages
{
    public const string French = "fr";
    public const string English = "en";
    public const string Default = French;

    /// <summary>Key used internally for texts that are identical in every language.</summary>
    public const string Invariant = "*";

    public static readonly string[] Supported = [French, English];

    public static CultureInfo Culture(string language) =>
        CultureInfo.GetCultureInfo(language == English ? "en-US" : "fr-FR");
}
