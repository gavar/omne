using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Morris.Blazor.Validation;
using MudBlazor;
using MudBlazor.Services;
using OMNE.Web;
using OMNE.Web.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Aspire
builder.AddServiceDefaults();

// API
builder.Services.AddSingleton(static provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var url = config.GetServiceEndpoints("api").First();
    return new OmneClient(url);
});

builder.Services.AddMudServices(options => options.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight);
builder.Services.AddFormValidation(config => config.AddDataAnnotationsValidation());

var host = builder.Build();
await host.RunAsync().ConfigureAwait(false);
