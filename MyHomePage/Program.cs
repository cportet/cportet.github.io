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

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddFluentUIComponents();
builder.Services.AddPWAUpdater();

builder.Services.AddScoped<LanguageService>();
builder.Services.AddScoped<ContentService>();
builder.Services.AddScoped<ThemePreferences>();

var host = builder.Build();

// The whole content is small: load it once, before the first render.
await host.Services.GetRequiredService<ContentService>().LoadAsync();

await host.RunAsync();
