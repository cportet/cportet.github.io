using System.Globalization;
using System.Net.Http.Json;

namespace MyHomePage.Extensions;

public static class HttpExtensions
{
    private const string DefaultCulture = "fr";
    private const string BasePath = "content";

    public static async Task<string?> LoadMarkdownAsync(this HttpClient http, string versionString, string ressourcePath, string? culture = null)
    {
        return await LoadResourceAsync(http, versionString, ressourcePath, "md", culture, async (client, path)
            => await client.GetStringAsync(path));
    }

    public static async Task<T?> LoadJsonAsync<T>(this HttpClient http, string versionString, string ressourcePath,  string? culture = null)
    {
        return await LoadResourceAsync<T>(http, versionString, ressourcePath, "json", culture, async (client, path)
            => await client.GetFromJsonAsync<T>(path));
    }

    private static async Task<T?> LoadResourceAsync<T>(HttpClient http, string versionString, string ressourcePath, string fileExtension, string? culture, Func<HttpClient, string, Task<T?>> loadFunc)
    {
        var currentCulture = culture ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var culturesToTry = currentCulture == DefaultCulture ? [currentCulture] : new[] { currentCulture, DefaultCulture };

        foreach (var cultureToTry in culturesToTry)
        {
            try
            {
                var path = $"{BasePath}/{ressourcePath}.{cultureToTry}.{fileExtension}?v={versionString}";
                return await loadFunc(http, path);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Culture '{cultureToTry}' not found for '{ressourcePath}' file");
            }
        }

        throw new FileNotFoundException($"Resource file not found for path '{ressourcePath}' in cultures '{string.Join(", ", culturesToTry)}'.");
    }
}
