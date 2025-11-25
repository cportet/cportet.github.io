using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MyImageDialog : IDialogContentComponent<MyImageDialog.ImageDialogContent>
{
    [CascadingParameter]
    public FluentDialog? Dialog { get; set; }

    [Parameter]
    public ImageDialogContent Content { get; set; } = null!;

    public static DialogParameters DefaultParameters => new()
    {
        Title = AppRessources.ImageDialog_Title,
        Height = "80dvh",
        Width = "800px",
        TrapFocus = false,
    };

    public sealed record ImageDialogContent(
        string SourceUrl,
        string? Title,
        string? AltText);
}
