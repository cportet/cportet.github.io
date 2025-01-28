using Microsoft.AspNetCore.Components;

namespace MyHomePage.Layout;

public partial class NavMenu
{
    [Parameter]
    public EventCallback OnItemClick { get; set; }

    private async Task ItemClickedHandler()
    {
        await OnItemClick.InvokeAsync();
    }
}