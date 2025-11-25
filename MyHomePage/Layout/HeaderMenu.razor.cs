using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using MyHomePage.Services;

namespace MyHomePage.Layout;

public partial class HeaderMenu(
    IJSRuntime js,
    HistoryService historyService)
{

    [Parameter]
    [EditorRequired]
    public DesignThemeModes Mode { get; set; }


    [Parameter]
    [EditorRequired]
    public EventCallback ToggleThemeRequested { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback ToggleMenuRequested { get; set; }

    [Parameter]
    [EditorRequired]
    public bool MenuVisible { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private const string SpacerWidth = "5px";
    private bool _canInstall;

    private string ToggleThemeTitle => Mode == DesignThemeModes.Light
        ? AppRessources.MainLayout_Theme_Sombre
        : AppRessources.MainLayout_Theme_Clair;

    private string ToggleMenuTitle => MenuVisible
        ? AppRessources.MainLayout_ToggleMenuTitle_Masquer
        : AppRessources.MainLayout_ToggleMenuTitle_Afficher;

    private string GoBackMenuTitle => historyService.CanGoBack()
        ? AppRessources.MainLayout_GoBack_Title
        : string.Empty;

    private void GoBack() =>
        historyService.GoBack();

    private async Task ToggleTheme()
    {
        if (ToggleThemeRequested.HasDelegate)
            await ToggleThemeRequested.InvokeAsync();
    }

    private async Task ToggleMenu()
    {
        if (ToggleMenuRequested.HasDelegate)
            await ToggleMenuRequested.InvokeAsync();
    }

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

        var consoleMessage = accepted
            ? "Pwa Installed"
            : "Pwa Not Installed";

        Console.WriteLine(consoleMessage);
    }
}
