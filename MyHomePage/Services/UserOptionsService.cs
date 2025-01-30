using MyHomePage.Code;

namespace MyHomePage.Services;
using System.Threading.Tasks;

public class UserOptionsService(
    AppConfig appConfig,
    LocalStorageService localStorageService)
{
    public async Task<UserOptions> GetUserOptionsAsync()
    {
        var language = await localStorageService.GetAsync("language");
        var theme = await localStorageService.GetAsync("theme");
        var colorSet = await localStorageService.GetAsync("colorset");

        return new UserOptions(
            language ?? appConfig.DefaultLanguage,
            theme ?? appConfig.DefaultTheme,
            colorSet ?? appConfig.DefaultOfficeColor);
    }

    public async Task SetUserLanguageAsync(string? language)
    {
        await localStorageService.SetAsync("language", language);
    }

    public async Task SetUserThemeAsync(string? theme)
    {
        await localStorageService.SetAsync("theme", theme);
    }

    public async Task SetUserColorsetAsync(string? colorset)
    {
        await localStorageService.SetAsync("colorset", colorset);
    }

    public async Task ClearUserOptionsAsync()
    {
        await localStorageService.DeleteAsync("language");
        await localStorageService.DeleteAsync("theme");
        await localStorageService.DeleteAsync("colorset");
    }
}

public sealed record UserOptions(string Language, string Theme, string ColorSet);
