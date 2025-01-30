using Microsoft.JSInterop;

namespace MyHomePage.Code;

public class LocalStorageService(IJSRuntime jsRuntime)
{
    public async Task<string?> GetAsync(string key)
    {
        return await jsRuntime
            .InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task SetAsync(string key, string? value)
    {
        if (value == null)
        {
            await jsRuntime
                .InvokeVoidAsync("localStorage.removeItem", key);
        }
        else
        {
            await jsRuntime
                .InvokeVoidAsync("localStorage.setItem", key, value);
        }
    }

    public async Task ClearAsync()
    {
        await jsRuntime
            .InvokeVoidAsync("localStorage.clear");
    }

    public async Task DeleteAsync(string key)
    {
        await jsRuntime
            .InvokeVoidAsync("localStorage.removeItem", key);
    }
}
