using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Theming;

public sealed record AccentOption(string Key, string Color);

/// <summary>
/// Light/dark mode and accent color, on top of the Fluent UI theme service
/// (which persists them in localStorage and re-applies them before the first render).
/// </summary>
public sealed class ThemePreferences(IThemeService theme)
{
    public static readonly AccentOption[] Accents =
    [
        new("blue", "#0F6CBD"),
        new("teal", "#0B7A75"),
        new("purple", "#6B4FBB"),
        new("red", "#C4314B"),
    ];

    public static string DefaultAccent => Accents[0].Color;

    public event Action? Changed;

    public async Task<ThemeSettings> GetAsync() =>
        await theme.GetThemeSettingsAsync()
        ?? new ThemeSettings(DefaultAccent, 0, 0, ThemeMode.System, false);

    public Task<bool> IsDarkAsync() => theme.IsDarkModeAsync();

    public async Task SetModeAsync(ThemeMode mode)
    {
        var current = await GetAsync();
        await ApplyAsync(new ThemeSettings(current.Color ?? DefaultAccent, current.HueTorsion, current.Vibrancy, mode, false));
    }

    public async Task SetAccentAsync(string color)
    {
        var current = await GetAsync();
        await ApplyAsync(new ThemeSettings(color, 0, 0, current.Mode, false));
    }

    public async Task ToggleDarkAsync() =>
        await SetModeAsync(await IsDarkAsync() ? ThemeMode.Light : ThemeMode.Dark);

    public async Task ResetAsync()
    {
        await theme.ClearStoredThemeSettingsAsync();
        await theme.SetThemeAsync(new ThemeSettings(DefaultAccent, 0, 0, ThemeMode.System, false));
        Changed?.Invoke();
    }

    private async Task ApplyAsync(ThemeSettings settings)
    {
        await theme.SetThemeAsync(settings);
        Changed?.Invoke();
    }
}
