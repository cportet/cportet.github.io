﻿using System.Globalization;
using System.Resources;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using MyHomePage;
using MyHomePage.Code;
using MyHomePage.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<ConfigurationService>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<UserOptionsService>();

builder.Services.AddFluentUIComponents();
builder.Services.AddPWAUpdater();
builder.Services.AddLocalization();

var appConfig = await builder.LoadConfig();

builder.Services.AddSingleton(appConfig);

var host = builder.Build();

var userOptionsService = host.Services.GetRequiredService<UserOptionsService>();
var options = await userOptionsService.GetUserOptionsAsync();
Cultures.ApplyCulture(options.Language);

await host.RunAsync();
