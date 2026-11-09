using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Theming;

/// <param name="Color">Hex color.</param>
/// <param name="UiKey">Key of its name in ui.json.</param>
public sealed record AccentOption(string Color, string UiKey);

/// <summary>
/// Light/dark mode and accent color, on top of the Fluent UI theme service
/// (which persists them in localStorage and re-applies them before the first render).
/// </summary>
public sealed class ThemePreferences(IThemeService theme)
{
    public static readonly AccentOption[] Accents =
    [
        new(Constants.Theme.Accents.Blue, Constants.Ui.Preferences.Accent.Blue),
        new(Constants.Theme.Accents.Teal, Constants.Ui.Preferences.Accent.Teal),
        new(Constants.Theme.Accents.Purple, Constants.Ui.Preferences.Accent.Purple),
        new(Constants.Theme.Accents.Red, Constants.Ui.Preferences.Accent.Red),
        new(Constants.Theme.Accents.Green, Constants.Ui.Preferences.Accent.Green),
        new(Constants.Theme.Accents.Orange, Constants.Ui.Preferences.Accent.Orange),
        new(Constants.Theme.Accents.Indigo, Constants.Ui.Preferences.Accent.Indigo),
        new(Constants.Theme.Accents.Magenta, Constants.Ui.Preferences.Accent.Magenta),
        new(Constants.Theme.Accents.Graphite, Constants.Ui.Preferences.Accent.Graphite),
    ];

    public event Action? Changed;

    public async Task<ThemeSettings> GetAsync() =>
        await theme.GetThemeSettingsAsync()
        ?? new ThemeSettings(Constants.Theme.Accents.Default, 0, 0, ThemeMode.System, false);

    public Task<bool> IsDarkAsync() => theme.IsDarkModeAsync();

    public async Task SetModeAsync(ThemeMode mode)
    {
        var current = await GetAsync();
        await ApplyAsync(new ThemeSettings(current.Color ?? Constants.Theme.Accents.Default, current.HueTorsion, current.Vibrancy, mode, false));
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
        await theme.SetThemeAsync(new ThemeSettings(Constants.Theme.Accents.Default, 0, 0, ThemeMode.System, false));
        Changed?.Invoke();
    }

    private async Task ApplyAsync(ThemeSettings settings)
    {
        await theme.SetThemeAsync(settings);
        Changed?.Invoke();
    }
}
