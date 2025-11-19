using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MyImageDisplay
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

    private static void ImageClickHandler()
    {
        Console.WriteLine("Image clicked");
    }

    private string ImageClass => Rounded
        ? "my-rounded"
        : "my-standard";
}
