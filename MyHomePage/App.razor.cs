using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace MyHomePage;

public partial class App(NavigationManager navigationManager,
    IJSRuntime jsRuntime) : IDisposable
{
    protected override void OnInitialized()
    {
        navigationManager.LocationChanged += OnLocationChanged;
    }

    private static void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        Console.WriteLine($"Location changed to {e.Location}");
    }

    public void Dispose()
    {
        navigationManager.LocationChanged -= OnLocationChanged;
        GC.SuppressFinalize(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await jsRuntime.InvokeVoidAsync("removeInitialDarkClass");

        await base.OnAfterRenderAsync(firstRender);
    }
}
