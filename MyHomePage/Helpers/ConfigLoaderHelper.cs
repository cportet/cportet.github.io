using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyHomePage.Code;

namespace MyHomePage.Helpers;

public static class ConfigLoaderHelper
{
    //private static readonly JsonSerializerOptions Options = new()
    //{
    //    PropertyNameCaseInsensitive = true,
    //    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    //};

    public static async Task<AppConfig> LoadConfig(this WebAssemblyHostBuilder builder)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

        //string jsonString = await httpClient.GetStringAsync("appconfig.json");
        //return JsonSerializer.Deserialize<AppConfig>(jsonString, Options)!;

        var result = await httpClient.GetFromJsonAsync<AppConfig>("appconfig.json");
        return result!;
    }
}
