using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MyHeaderTitle
{
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? PageTitle { get; set; }
}