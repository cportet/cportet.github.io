using Microsoft.JSInterop;

namespace MyHomePage.Layout;

public partial class HeaderMenu(IJSRuntime js)
{
    private bool _canInstall;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_canInstall)
        {
            _canInstall = await js.InvokeAsync<bool>("pwaInstall.canInstall");
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task PwaInstall()
    {
        var accepted = await js.InvokeAsync<bool>("pwaInstall.showInstallPrompt");
        if (accepted)
            Console.WriteLine("Pwa Installed");
        else
            Console.WriteLine("Pwa Not Installed");
    }
}
