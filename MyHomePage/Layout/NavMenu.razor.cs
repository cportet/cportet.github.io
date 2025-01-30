using Microsoft.AspNetCore.Components;
using MyHomePage.Code;

namespace MyHomePage.Layout;

public partial class NavMenu(AppConfig appConfig)
{
    [Parameter]
    public EventCallback OnItemClick { get; set; }

    private async Task ItemClickedHandler()
    {
        await OnItemClick.InvokeAsync();
    }
}
