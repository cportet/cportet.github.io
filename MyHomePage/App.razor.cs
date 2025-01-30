using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MyHomePage;

public partial class App(NavigationManager navigationManager) : IDisposable
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
}
