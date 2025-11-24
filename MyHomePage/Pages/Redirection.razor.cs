using Microsoft.AspNetCore.Components;

namespace MyHomePage.Pages;

[Route("redirection")]
[Route("redir")]
public partial class Redirection(NavigationManager nav)
{
    [Parameter]
    [SupplyParameterFromQuery]
    public string? Path { get; set; }

    protected override void OnParametersSet()
    {
        var target = !string.IsNullOrWhiteSpace(Path)
            ? Uri.UnescapeDataString(Path)
            : "/";

        target = string.IsNullOrWhiteSpace(target)
            ? "/"
            : target;

        Console.WriteLine($"Redirection to {target}");
        nav.NavigateTo(target);
    }
}
