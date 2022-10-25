using Compass.Wasm.CommonInitializer;
using Compass.Wasm.FileService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//基础配置
builder.ConfigureDbConfiguration();
builder.ConfigureExtraServices(new InitializerOptions()
{
    LogFilePath = "d:/compass.log/compass.wasm.fileservice.log",
    EventBusQueueName = "Compass.Wasm.FileService.WebApi"
});
//文件服务
builder.Services.Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseCompassDefault();

app.MapControllers();

app.Run();
