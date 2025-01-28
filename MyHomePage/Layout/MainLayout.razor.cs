using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Layout;

public partial class MainLayout : LayoutComponentBase
{
    private DesignThemeModes _mode = DesignThemeModes.Dark;
    private OfficeColor? _selectedOfficeColor = OfficeColor.OneDrive;
    private bool _menuVisible = false;
    private string? _pageTitle;

    private string ToggleMenuTitle => _menuVisible
        ? "Masquer le menu"
        : "Afficher le menu";

    private string ToggleThemeTitle => _mode == DesignThemeModes.Light
        ? "Thème sombre"
        : "Thème clair";

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
}