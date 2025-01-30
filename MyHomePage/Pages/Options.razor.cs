using Microsoft.AspNetCore.Components;
using MyHomePage.Code;
using MyHomePage.Layout;
using MyHomePage.Services;

namespace MyHomePage.Pages;

[Route("options")]
public partial class Options(UserOptionsService userOptionsService)
{
    private UserOptions? _options;

    [CascadingParameter]
    private MainLayout MainLayout { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _options = await userOptionsService.GetUserOptionsAsync();
        await base.OnInitializedAsync();
    }

    private async Task CultureUpdate(string value)
    {

        await userOptionsService.SetUserLanguageAsync(value);
        _options = await userOptionsService.GetUserOptionsAsync();
        Cultures.ApplyCulture(_options.Language);

        MainLayout.UpdateState();
    }
}
