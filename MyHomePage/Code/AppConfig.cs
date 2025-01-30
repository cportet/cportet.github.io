using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Code;

public sealed record AppConfig(
    string Title,
    string DefaultTheme,
    string DefaultOfficeColor,
    DateTime BuildTimestamp)
{
    public DesignThemeModes Theme =>
        DefaultTheme.ToEnum(DesignThemeModes.Dark);

    public OfficeColor OfficeColor =>
        DefaultOfficeColor.ToEnum(OfficeColor.OneDrive);

    public string VersionString => $"Version {BuildTimestamp:yyyy.MM.dd.HHmmss}";
}

