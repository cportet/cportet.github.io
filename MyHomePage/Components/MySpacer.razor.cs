using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MySpacer
{
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    [Parameter]
    public string Space { get; set; } = "10px";

    private string StyleString => Orientation == Orientation.Vertical
    ? $"width:{Space};"
    : $"height:{Space};";
}