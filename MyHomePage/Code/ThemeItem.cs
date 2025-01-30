using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace MyHomePage.Code;

public sealed record ThemeItem(DesignThemeModes Theme) : ISelectItem
{
    public string SelectText => Theme.GetDisplayName();
    public string SelectKey => Theme.ToString().ToLower();
}
