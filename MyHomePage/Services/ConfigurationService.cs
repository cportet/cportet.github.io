using System.Net.Http.Json;
using MyHomePage.Code;

namespace MyHomePage.Services;

public class ConfigurationService(HttpClient httpClient)
{
    private AppConfig? _config;

    public async Task<AppConfig> GetConfigurationAsync() => _config ??=
               (await httpClient.GetFromJsonAsync<AppConfig>("appconfig.json"))!;
}
