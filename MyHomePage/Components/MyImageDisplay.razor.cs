using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MyImageDisplay
{
    [Parameter]
    [EditorRequired]
    public string? ImageUrl { get; set; }

    [Parameter]
    public string? AltText { get; set; }

    [Parameter]
    public string Width { get; set; } = "200px";

    [Parameter]
    public string Height { get; set; } = "200px";
}