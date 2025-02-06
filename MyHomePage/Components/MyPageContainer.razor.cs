using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MyPageContainer
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? SubTitle { get; set; }
}
