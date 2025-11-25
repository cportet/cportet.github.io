using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MyImageDisplay(
    IDialogService dialogService)
{
    [Parameter]
    [EditorRequired]
    public string? ImageUrl { get; set; }

    [Parameter]
    public string? AltText { get; set; }

    [Parameter]
    public string Width { get; set; } = "200px";

    [Parameter]
    public string Height { get; set; } = "200px";

    [Parameter]
    public bool Rounded { get; set; } = true;

    private async Task ImageClickHandler()
    {

        if (!string.IsNullOrWhiteSpace(ImageUrl))
        {
            Console.WriteLine("Image clicked");

            var imageContent = new MyImageDialog.ImageDialogContent(
                SourceUrl: ImageUrl,
                Title: AltText,
                AltText: AltText);

            DialogParameters parameters = MyImageDialog.DefaultParameters;

            await dialogService.ShowDialogAsync<MyImageDialog>(imageContent, parameters);
        }
    }

    private string ImageClass => Rounded
        ? "my-rounded"
        : "my-standard";
}
