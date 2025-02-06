using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using MyHomePage.Services;

namespace MyHomePage;

public partial class App(NavigationManager navigationManager,
    IJSRuntime jsRuntime,
    HistoryService historyService
    ) : IDisposable
{
    private bool _isInitialized;
    protected override void OnInitialized()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            navigationManager.LocationChanged += OnLocationChanged;
            historyService.Add(navigationManager.Uri);
        }
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        Console.WriteLine($"Location changed to {e.Location}");
        historyService.Add(e.Location);
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
