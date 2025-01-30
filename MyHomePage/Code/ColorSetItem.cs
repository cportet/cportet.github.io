using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace MyHomePage.Code;

public sealed record ColorSetItem(OfficeColor ColorSet) : ISelectItem
{
    public string SelectText => ColorSet.GetDisplayName();
    public string SelectKey => ColorSet.ToString().ToLower();
}
