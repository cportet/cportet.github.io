using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using MyHomePage.Code;
using MyHomePage.Services;

namespace MyHomePage.Layout;

public partial class MainLayout(
    AppConfig appConfig,
    UserOptionsService userOptionsService,
    HistoryService historyService) : LayoutComponentBase
{
    private DesignThemeModes _mode;
    private OfficeColor _selectedOfficeColor;
    private UserOptions _options = null!;

    private bool _userOptionsInitialized;
    private bool _menuVisible;
    private string? _pageTitle;

    private string ToggleMenuTitle => _menuVisible
        ? AppRessources.MainLayout_ToggleMenuTitle_Masquer
        : AppRessources.MainLayout_ToggleMenuTitle_Afficher;

    private string ToggleThemeTitle => _mode == DesignThemeModes.Light
        ? AppRessources.MainLayout_Theme_Sombre
        : AppRessources.MainLayout_Theme_Clair;

    private string GoBackMenuTitle => historyService.CanGoBack()
        ? AppRessources.MainLayout_GoBack_Title
        : string.Empty;

    private void GoBack() =>
        historyService.GoBack();

    private void ToggleMenu() =>
        _menuVisible = !_menuVisible;

    private async Task ToggleTheme()
    {
        _mode = _mode == DesignThemeModes.Light
        ? DesignThemeModes.Dark
        : DesignThemeModes.Light;

        var modeString = _mode.ToString().ToLower();

        await userOptionsService.SetUserThemeAsync(modeString);
        await ApplyTheme();
    }

    private void NavMenuOnItemClick() =>
        _menuVisible = false;

    private void BodyClick() =>
        _menuVisible = false;

    public void SetPageTitle(string? pageTitle)
    {
        if (pageTitle != _pageTitle)
        {
            _pageTitle = pageTitle;
            StateHasChanged();
        }
    }

    public async Task UpdateState()
    {
        await ApplyTheme();
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await ApplyTheme();
        await base.OnInitializedAsync();
    }

    private async Task ApplyTheme()
    {
        _options = await userOptionsService
            .GetUserOptionsAsync();

        _selectedOfficeColor = _options.ColorSet.ToEnum(appConfig.OfficeColor);
        _mode = _options.Theme.ToEnum(appConfig.Theme);

        _userOptionsInitialized = true;
    }
}
