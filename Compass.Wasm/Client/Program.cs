using Blazored.LocalStorage;
using Compass.Wasm.Client;
using Compass.Wasm.Client.IdentityService;
using Compass.Wasm.Client.ProjectService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Blazor授权
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage();//浏览器LocalStorage，用于存储和读取token

builder.Services.AddScoped<ITrackingRepository, TrackingRepository>();


await builder.Build().RunAsync();
