using DataModels.Services;
using DataModels.Services.Interface;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using WebApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? string.Empty) });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<HelperClass>();
builder.Services.AddScoped<DialogService>();

await builder.Build().RunAsync();
