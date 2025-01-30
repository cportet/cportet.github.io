using Microsoft.AspNetCore.Components;
using MyHomePage.Code;
using MyHomePage.Layout;
using MyHomePage.Services;

namespace MyHomePage.Pages;

[Route("options")]
public partial class Options(
    UserOptionsService userOptionsService,
    NavigationManager navigationManager)
{
    private UserOptions? _options;
    private readonly List<CultureItem> _availableLanguages = Cultures.GetCultures();
    private readonly List<ThemeItem> _availableThemes = Themes.GetThemes();
    private readonly List<ColorSetItem> _availableColorSets = ColorSets.GetColorSets();

    [CascadingParameter]
    private MainLayout MainLayout { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _options = await userOptionsService.GetUserOptionsAsync();
        await base.OnInitializedAsync();
    }

    private async Task ThemeUpdate(string value)
    {
        await userOptionsService.SetUserThemeAsync(value);
        _options = await userOptionsService.GetUserOptionsAsync();

        await MainLayout.UpdateState();
    }

    private async Task ColorSetUpdate(string value)
    {
        await userOptionsService.SetUserColorsetAsync(value);
        _options = await userOptionsService.GetUserOptionsAsync();
        await MainLayout.UpdateState();
    }

    private async Task CultureUpdate(string value)
    {
        await userOptionsService.SetUserLanguageAsync(value);
        _options = await userOptionsService.GetUserOptionsAsync();
        Cultures.ApplyCulture(_options.Language);

        await MainLayout.UpdateState();
    }

    private void ReloadHandler()
    {
        navigationManager
            .NavigateTo(navigationManager.Uri, true);
    }

    private async Task ResetHandler()
    {
        await userOptionsService.ClearUserOptionsAsync();
        ReloadHandler();
    }
}
