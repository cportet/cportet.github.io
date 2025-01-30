namespace MyHomePage.Code;

public sealed record FileReference(
    FileKind Kind,
    string Title,
    string SubTitle,
    string DownloadTitle,
    string FilePath,
    string FileType,
    string DownloadFileName)
{
    public static FileReference? FromFileKind(FileKind? kind)
    {
        return kind switch
        {
            FileKind.CV => new FileReference(
                FileKind.CV,
                 AppRessources.File_FileKind_CV_Title,
                 AppRessources.File_FileKind_CV_SubTitle,
                 AppRessources.File_FileKind_CV_DownloadTitle,
                "doc/cv-cyril-portet.pdf",
                "application/pdf",
                "cv-cyril-portet.pdf"),
            _ => null
        };
    }

    public static FileReference? FromString(string? kind)
    {
        var formattedKind = kind?.Trim().ToLower();

        return formattedKind switch
        {
            "cv" => FromFileKind(FileKind.CV),
            _ => null
        };
    }

}
