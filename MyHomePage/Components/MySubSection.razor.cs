using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MySubSection
{
    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? SubTitle { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
