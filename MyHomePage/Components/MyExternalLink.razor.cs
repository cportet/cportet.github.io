using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MyExternalLink
{
    [Parameter]
    [EditorRequired]
    public string Title { get; set; } = null!;

    [Parameter]
    [EditorRequired]
    public string Href { get; set; } = null!;

    [Parameter]
    public string? Alt { get; set; }

    private string AltText => Alt ?? Title;
}
