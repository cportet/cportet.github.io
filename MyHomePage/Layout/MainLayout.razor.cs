using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace MyHomePage.Layout;

public partial class MainLayout
{
    private DesignThemeModes _mode = DesignThemeModes.Dark;
    private OfficeColor? _selectedOfficeColor  = OfficeColor.OneDrive;
    private bool _menuVisible = false;

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
}