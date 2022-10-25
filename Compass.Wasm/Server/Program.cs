using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Compass.FileService.Domain;
using Compass.FileService.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Zack.ASPNETCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Zack.EventBus;
using Zack.JWT;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region 【中心配置数据库】的连接字符串
builder.Host.ConfigureAppConfiguration((hostCtx, configBuilder) =>
{
    //从环境变量中获取连接字符串
    string connStr = builder.Configuration.GetValue<string>("DefaultDB:ConnStr");
    //NuGet安装：Install-Package Zack.AnyDBConfigProvider
    configBuilder.AddDbConfiguration(() => new SqlConnection(connStr), reloadOnChange: true, reloadInterval: TimeSpan.FromSeconds(5));
});
#endregion

#region FileService
//数据库，DbContext
builder.Services.AddDbContext<FSDbContext>(options =>
{
    //指定连接的数据库
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
//文件服务(本地存储)
builder.Services.Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"));
//HttpContextAccessor 默认实现了它简化了访问HttpContext。用来访问HttpContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IStorageClient, SMBStorageClient>();//本机磁盘当备份服务器
builder.Services.AddScoped<IStorageClient, MockCloudStorageClient>();//文件保存在wwwroot文件夹下
builder.Services.AddScoped<IFSRepository, FSRepository>();
builder.Services.AddScoped<FSDomainService>();

#endregion

#region 工作单元
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());//传递当前运行的程序集
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<UnitOfWorkFilter>();
});
#endregion

#region FluentValidation请求数据校验
//NuGet安装：Install-Package FluentValidation.AspNetCore
//配置FluentValidation
builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssembly(Assembly.GetEntryAssembly());
#endregion

#region 配置Serilog日志
//NuGet安装：Install-Package Serilog.AspNetCore
//配置Serilog日志，LogFilePath从调用者（program）初始化InitializerOptions而来
builder.Services.AddLogging(loggingBuilder =>
{
    Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Information().Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("d:/compass.log/compass.wasm.log") //记录日志到调用者初始化设定的位置
        .CreateLogger();
    loggingBuilder.AddSerilog();
});
#endregion

#region Authentication,Authorization
//NuGet安装：Install-Package Zack.JWT
//开始:Authentication,Authorization
//只要需要校验Authentication报文头的地方（非IdentityService.WebAPI项目）也需要启用这些
//IdentityService项目还需要启用AddIdentityCore
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
JWTOptions jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
builder.Services.AddJWTAuthentication(jwtOpt);
//启用Swagger中的【Authorize】按钮。这样就不用每个项目的AddSwaggerGen中单独配置了
builder.Services.Configure<SwaggerGenOptions>(c =>
{
    c.AddAuthenticationHeader();
});
//配置JWT的选项，包含secKey和过期时间
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
#endregion

#region RabbitMQ
//NuGet,安装Install-Package Zack.EventBus
//配置RabbitMQ，从中心化服务器读取RabbitMQ的服务器地址和交换机名
builder.Services.Configure<IntegrationEventRabbitMQOptions>(builder.Configuration.GetSection("RabbitMQ"));
//配置消息队列名
builder.Services.AddEventBus("Compass.Wasm", Assembly.GetExecutingAssembly());
#endregion

#region Redis
//Redis未开启？wsl
//sudo service redis-server start
//redis-cli
//ping
//keys *
//配置Redis的服务器，分布式缓存
string redisConnStr = builder.Configuration.GetValue<string>("Redis:ConnStr");
IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
builder.Services.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);

#endregion

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Install-Package Swashbuckle.AspNetCore
builder.Services.AddSwaggerGen();

//修改默认编译发布后的5000，5001启动端口
//builder.WebHost.UseUrls(new[] {"http://*:80" });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseWebAssemblyDebugging();

#region 中间件
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();
app.UseEventBus();
app.UseAuthentication();//JWT
app.UseAuthorization();
#endregion

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
