namespace MyHomePage.Content;

/// <summary>Build information, stamped by the deployment workflow into wwwroot/appconfig.json.</summary>
public sealed record AppConfig(DateTime BuildTimestamp)
{
    public string Version => $"{BuildTimestamp:yyyy.MM.dd}";
}
