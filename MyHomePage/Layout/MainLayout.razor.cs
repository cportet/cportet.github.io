using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using MyHomePage.Code;

namespace MyHomePage.Layout;

public partial class MainLayout(AppConfig config) : LayoutComponentBase
{
    private DesignThemeModes _mode = config.Theme;
    private readonly OfficeColor? _selectedOfficeColor = config.OfficeColor;
    private bool _menuVisible;
    private string? _pageTitle;

    private string ToggleMenuTitle => _menuVisible
        ? AppRessources.MainLayout_ToggleMenuTitle_Masquer
        : AppRessources.MainLayout_ToggleMenuTitle_Afficher;

    private string ToggleThemeTitle => _mode == DesignThemeModes.Light
        ? AppRessources.MainLayout_Theme_Sombre
        : AppRessources.MainLayout_Theme_Clair;

    private void ToggleMenu()
    {
        _menuVisible = !_menuVisible;
    }

    private void ToggleTheme()
    {
        _mode = _mode == DesignThemeModes.Light
        ? DesignThemeModes.Dark
        : DesignThemeModes.Light;
    }

    private void NavMenuOnItemClick()
    {
        _menuVisible = false;
    }

    private void BodyClick()
    {
        _menuVisible = false;
    }

    public void SetPageTitle(string? pageTitle)
    {
        if (pageTitle != _pageTitle)
        {
            _pageTitle = pageTitle;
            StateHasChanged();
        }
    }

    public void UpdateState()
    {
        StateHasChanged();
    }
}
