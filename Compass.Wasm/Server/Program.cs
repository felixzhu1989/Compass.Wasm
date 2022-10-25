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

#region �������������ݿ⡿�������ַ���
builder.Host.ConfigureAppConfiguration((hostCtx, configBuilder) =>
{
    //�ӻ��������л�ȡ�����ַ���
    string connStr = builder.Configuration.GetValue<string>("DefaultDB:ConnStr");
    //NuGet��װ��Install-Package Zack.AnyDBConfigProvider
    configBuilder.AddDbConfiguration(() => new SqlConnection(connStr), reloadOnChange: true, reloadInterval: TimeSpan.FromSeconds(5));
});
#endregion

#region FileService
//���ݿ⣬DbContext
builder.Services.AddDbContext<FSDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
//�ļ�����(���ش洢)
builder.Services.Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"));
//HttpContextAccessor Ĭ��ʵ���������˷���HttpContext����������HttpContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IStorageClient, SMBStorageClient>();//�������̵����ݷ�����
builder.Services.AddScoped<IStorageClient, MockCloudStorageClient>();//�ļ�������wwwroot�ļ�����
builder.Services.AddScoped<IFSRepository, FSRepository>();
builder.Services.AddScoped<FSDomainService>();

#endregion

#region ������Ԫ
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());//���ݵ�ǰ���еĳ���
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<UnitOfWorkFilter>();
});
#endregion

#region FluentValidation��������У��
//NuGet��װ��Install-Package FluentValidation.AspNetCore
//����FluentValidation
builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssembly(Assembly.GetEntryAssembly());
#endregion

#region ����Serilog��־
//NuGet��װ��Install-Package Serilog.AspNetCore
//����Serilog��־��LogFilePath�ӵ����ߣ�program����ʼ��InitializerOptions����
builder.Services.AddLogging(loggingBuilder =>
{
    Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Information().Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("d:/compass.log/compass.wasm.log") //��¼��־�������߳�ʼ���趨��λ��
        .CreateLogger();
    loggingBuilder.AddSerilog();
});
#endregion

#region Authentication,Authorization
//NuGet��װ��Install-Package Zack.JWT
//��ʼ:Authentication,Authorization
//ֻҪ��ҪУ��Authentication����ͷ�ĵط�����IdentityService.WebAPI��Ŀ��Ҳ��Ҫ������Щ
//IdentityService��Ŀ����Ҫ����AddIdentityCore
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
JWTOptions jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
builder.Services.AddJWTAuthentication(jwtOpt);
//����Swagger�еġ�Authorize����ť�������Ͳ���ÿ����Ŀ��AddSwaggerGen�е���������
builder.Services.Configure<SwaggerGenOptions>(c =>
{
    c.AddAuthenticationHeader();
});
//����JWT��ѡ�����secKey�͹���ʱ��
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
#endregion

#region RabbitMQ
//NuGet,��װInstall-Package Zack.EventBus
//����RabbitMQ�������Ļ���������ȡRabbitMQ�ķ�������ַ�ͽ�������
builder.Services.Configure<IntegrationEventRabbitMQOptions>(builder.Configuration.GetSection("RabbitMQ"));
//������Ϣ������
builder.Services.AddEventBus("Compass.Wasm", Assembly.GetExecutingAssembly());
#endregion

#region Redis
//Redisδ������wsl
//sudo service redis-server start
//redis-cli
//ping
//keys *
//����Redis�ķ��������ֲ�ʽ����
string redisConnStr = builder.Configuration.GetValue<string>("Redis:ConnStr");
IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
builder.Services.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);

#endregion

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Install-Package Swashbuckle.AspNetCore
builder.Services.AddSwaggerGen();

//�޸�Ĭ�ϱ��뷢�����5000��5001�����˿�
//builder.WebHost.UseUrls(new[] {"http://*:80" });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseWebAssemblyDebugging();

#region �м��
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
