using System.Globalization;
using Microsoft.AspNetCore.Components;
using MyHomePage.Code;
using MyHomePage.Extensions;

namespace MyHomePage.Components;

public partial class MyMarkdownLoader(
    HttpClient http,
    AppConfig appConfig)
{
    [Parameter]
    [EditorRequired]
    public string? RessourcePath { get; set; }

    [Parameter]
    public Dictionary<string, string>? Variables { get; set; }

    private string? _previousRessourcesPath;
    private string? _previousCulture;

    private string? _content;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (string.IsNullOrEmpty(RessourcePath))
            return;

        var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        if (_previousRessourcesPath != RessourcePath || _previousCulture != currentCulture)
        {
            _previousCulture = currentCulture;
            _previousRessourcesPath = RessourcePath;

            _content = await LoadContentAsync(RessourcePath, currentCulture);
        }
    }

    private async Task<string?> LoadContentAsync(string ressourcePath, string culture) =>
        await http.LoadMarkdownAsync(appConfig.VersionUrlString, ressourcePath, culture);
}
