using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace MyHomePage.Layout;

public partial class MainLayout
{
    private DesignThemeModes Mode { get; set; } = DesignThemeModes.Dark;
    private OfficeColor? SelectedOfficeColor { get; set; } = OfficeColor.OneDrive;

    private Icon MenuIcon => new Size24.Navigation();
    private Icon ThemeIcon => new Size24.DarkTheme();

    private bool _menuVisible = true;

    private string ToggleMenuTitle => _menuVisible
        ? "Masquer le menu"
        : "Afficher le menu";

    private string ToggleThemeTitle => Mode == DesignThemeModes.Light
        ? "Thème sombre"
        : "Thème clair";

    private void ToggleMenu()
    {
        _menuVisible = !_menuVisible;
    }

    private void ToggleTheme()
    {
        Mode = Mode == DesignThemeModes.Light
        ? DesignThemeModes.Dark
        : DesignThemeModes.Light;
    }
}