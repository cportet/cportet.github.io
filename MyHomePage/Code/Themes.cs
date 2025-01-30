using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Code;

public static class Themes
{
    public static List<ThemeItem> GetThemes()
    {
        List<ThemeItem> themes = [];
        themes.AddRange(
            from DesignThemeModes mode
                in Enum.GetValues<DesignThemeModes>()
            select new ThemeItem(mode));

        return themes;
    }
}
