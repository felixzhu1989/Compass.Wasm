using Blazored.LocalStorage;
using Compass.Wasm.Client;
using Compass.Wasm.Client.Services.Categories;
using Compass.Wasm.Client.Services.Identities;
using Compass.Wasm.Client.Services.Plans;
using Compass.Wasm.Client.Services.Projects;
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
#region Categories
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IModelService,ModelService>();
builder.Services.AddScoped<IModelTypeService,ModelTypeService>();

#endregion

#region Projects
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IDrawingService, DrawingService>();
builder.Services.AddScoped<ILessonService, LessonService>();


#endregion

#region Plans
builder.Services.AddScoped<IMainPlanService, MainPlanService>();
builder.Services.AddScoped<IIssueService, IssueService>();


#endregion



builder.Services.AddScoped<IUserService, UserService>();
await builder.Build().RunAsync();
