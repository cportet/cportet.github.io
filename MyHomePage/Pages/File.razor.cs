using Microsoft.AspNetCore.Components;
using MyHomePage.Code;

namespace MyHomePage.Pages;

[Route("/file/{Kind}")]
public partial class File
{
    private string? _previousKind;
    private string? _title;
    private string? _subTitle;

    private FileReference? _fileReference;

    [Parameter]
    public string? Kind { get; set; }

    private void SetCurrentFileKind()
    {
        var formattedKind = Kind?.Trim().ToLower();

        if (formattedKind == _previousKind)
            return;

        _previousKind = formattedKind;
        _fileReference = FileReference.FromString(Kind);
        _title = _fileReference?.Title ?? "Document inconnu";
        _subTitle = _fileReference?.SubTitle ?? "Le document demandé n'existe pas";
    }

    protected override void OnParametersSet()
    {
        SetCurrentFileKind();
    }

}