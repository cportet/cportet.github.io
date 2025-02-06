using System.Globalization;
using Microsoft.AspNetCore.Components;
using MyHomePage.Code;

namespace MyHomePage.Components;

public partial class MyMarkdownLoader(
    HttpClient http,
    AppConfig appConfig)
{
    [Parameter]
    [EditorRequired]
    public string? RessourcePath { get; set; }

    private string? _previousRessourcesPath;
    private string? _previousCulture;

    private string? _content;

    private const string BasePath = "/content/";
    private const string DefaultCulture = "fr";

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (string.IsNullOrEmpty(RessourcePath))
            return;

        var currentCulture = CultureInfo.CurrentUICulture.Name;

        if (_previousRessourcesPath != RessourcePath || _previousCulture != currentCulture)
        {
            _previousCulture = currentCulture;
            _previousRessourcesPath = RessourcePath;

            _content = await LoadContentAsync(RessourcePath, currentCulture);
        }
    }

    private async Task<string?> LoadContentAsync(string ressourcePath, string culture)
    {
        var culturesToTry = culture == DefaultCulture ? [culture] : new[] { culture, DefaultCulture };

        foreach (var cultureToTry in culturesToTry)
        {
            try
            {
                return await http.GetStringAsync(
                    $"{BasePath}/{ressourcePath}.{cultureToTry}.md?v={appConfig.VersionUrlString}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Culture '{cultureToTry}' not found for '{ressourcePath}' file");
            }
        }

        throw new FileNotFoundException($"Markdown file not found for path '{ressourcePath}' in cultures '{string.Join(", ", culturesToTry)}'.");
    }


}
