using FSH.BlazorWebAssembly.Client.Infrastructure;
using FSH.BlazorWebAssembly.Client.Infrastructure.Common;
using FSH.BlazorWebAssembly.Client.Infrastructure.Preferences;
using Microsoft.Extensions.Hosting;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddClientServices(builder.Configuration);

var app = builder.Build();

//var storageService = app.Services.GetRequiredService<IClientPreferenceManager>();
//if (storageService != null)
//{
//    CultureInfo culture;
//    if (await storageService.GetPreference() is ClientPreference preference)
//        culture = new CultureInfo(preference.LanguageCode);
//    else
//        culture = new CultureInfo(LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US");
//    CultureInfo.DefaultThreadCurrentCulture = culture;
//    CultureInfo.DefaultThreadCurrentUICulture = culture;
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
