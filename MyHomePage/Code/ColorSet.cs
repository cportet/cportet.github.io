using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Code;

public static class ColorSets
{
    public static List<ColorSetItem> GetColorSets()
    {
        List<ColorSetItem> colorSet = [];
        colorSet.AddRange(
            from OfficeColor color
                in Enum.GetValues<OfficeColor>()
            select new ColorSetItem(color));

        return colorSet;
    }
}
