namespace MyHomePage.Code;

public sealed record FileReference(
    FileKind Kind,
    string Title,
    string SubTitle,
    string FilePath,
    string FileType,
    string DownloadFileName,
    string DownloadTitle)
{
    public static FileReference? FromFileKind(FileKind? kind)
    {
        return kind switch
        {
            FileKind.CV => new FileReference(
                FileKind.CV,
                "Mon CV",
                "Curriculum vitae de Cyril Portet",
                "doc/cv-cyril-portet.pdf",
                "application/pdf",
                "cv-cyril-portet.pdf",
                "Télécharger le CV (Format PDF)"),
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
