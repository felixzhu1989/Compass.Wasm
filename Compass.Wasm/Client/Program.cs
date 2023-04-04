using Blazored.LocalStorage;
using Compass.Wasm.Client;
using Compass.Wasm.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Blazor��Ȩ
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage();//�����LocalStorage�����ڴ洢�Ͷ�ȡtoken



//ע�����
builder.Services.AddScoped<IIndexService, IndexService>();
builder.Services.AddScoped<IProjectService,ProjectService>();
builder.Services.AddScoped<IDrawingService, DrawingService>();

await builder.Build().RunAsync();
