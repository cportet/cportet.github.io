using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MySpacer
{
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    [Parameter]
    public string Space { get; set; } = "10px";

    [Parameter]
    public string? Class { get; set; }

    private string StyleString => Orientation == Orientation.Vertical
    ? $"width:{Space};"
    : $"height:{Space};";

    private string ClassString
    {
        get
        {
            var baseClass = Orientation == Orientation.Vertical
                ? "my-v-spacer"
                : "my-h-spacer";

            if (!string.IsNullOrEmpty(Class))
                baseClass += " " + Class;

            return baseClass.Trim();
        }
    }
}
