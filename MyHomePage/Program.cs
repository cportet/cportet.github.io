using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using MyHomePage;
using MyHomePage.Content;
using MyHomePage.Theming;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress);

var host = builder.Build();

// The whole content is small: load it once, before the first render.
await host.Services.GetRequiredService<ContentService>().EnsureLoadedAsync();

await host.RunAsync();

// Also called by BlazorWasmPreRendering.Build, which renders every page to static HTML at publish time.
static void ConfigureServices(IServiceCollection services, string baseAddress)
{
    services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(baseAddress) });

    services.AddFluentUIComponents();
    services.AddPWAUpdater();

    services.AddScoped<LanguageService>();
    services.AddScoped<ContentService>();
    services.AddScoped<ThemePreferences>();
}
