using Compass.DataService.Domain;
using Compass.DataService.Infrastructure;
using Compass.PlanService.Domain;
using Compass.PlanService.Infrastructure;
using Compass.QualityService.Domain;
using Compass.QualityService.Infrastructure;
using Compass.TodoService.Domain;
using Compass.TodoService.Infrastructure;
using Compass.Wasm.Server.ExportExcel;
using Compass.Wasm.Server.TodoService;

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
//����Serilog��־
builder.Services.AddLogging(loggingBuilder =>
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Warning()
        .Enrich.FromLogContext()
        //.WriteTo.Console()
        .WriteTo.File($"d:/compass.log/compass.wasm.log") //��¼��־���ļ�
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
//����123
//redis-cli
//ping
//keys *
//����http://127.0.0.1:15672/#/
//����Redis�ķ��������ֲ�ʽ����
string redisConnStr = builder.Configuration.GetValue<string>("Redis:ConnStr");
IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
builder.Services.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);

#endregion

#region AutoMapper
//Install-Package AutoMapper
//Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
//https://dev.to/moe23/add-automapper-to-net-6-3fdn
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region FileService
//���ݿ⣬DbContext
builder.Services.AddDbContext<FileDbContext>(options =>
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
builder.Services.AddScoped<FileDomainService>();
builder.Services.AddScoped<IFileRepository, FileRepository>();

#endregion

#region IdentityService
//���ݿ⣬DbContext
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<IdentityDomainService>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddDataProtection();
//��¼��ע�����Ŀ����Ҫ����WebApplicationBuilderExtensions�еĳ�ʼ��֮�⣬��Ҫ���µĳ�ʼ��
//��Ҫ��AddIdentity��������AddIdentityCore
//��Ϊ��AddIdentity�ᵼ��JWT���Ʋ������ã�AddJwtBearer�лص����ᱻִ�У��������AuthenticationУ��ʧ��
//https://github.com/aspnet/Identity/issues/1376
IdentityBuilder idBuilder = builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 2;
    options.Password.RequireLowercase = false;
    //�����趨RequireUniqueEmail��������������Ϊ��
    //options.User.RequireUniqueEmail = true;
    //�������У���GenerateEmailConfirmationTokenAsync��֤������
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});
idBuilder = new IdentityBuilder(idBuilder.UserType, typeof(Role), builder.Services);
idBuilder.AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders()
    .AddRoleManager<RoleManager<Role>>()
    .AddUserManager<IdentityUserManager>();
//�����ʼ�
builder.Services.AddScoped<IEmailSender, EmailSender>();
//��ȡ���ķ������е�smtp����
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

#endregion

#region CategoryService
//���ݿ⣬DbContext
builder.Services.AddDbContext<CategoryDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<CategoryDomainService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
#endregion

#region ProjectService
//���ݿ⣬DbContext
builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<ProjectDomainService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
#endregion

#region DataService
//���ݿ⣬DbContext
builder.Services.AddDbContext<DataDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<DataDomainService>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
#endregion

#region PlanService
//���ݿ⣬DbContext
builder.Services.AddDbContext<PlanDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<PlanDomainService>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
#endregion

#region QualityService
//���ݿ⣬DbContext
builder.Services.AddDbContext<QualityDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<QualityDomainService>();
builder.Services.AddScoped<IQualityRepository, QualityRepository>();
#endregion

#region TodoService
//���ݿ⣬DbContext
builder.Services.AddDbContext<TodoDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<TodoDomainService>();
builder.Services.AddScoped<ITodoRepository,TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IMemoService, MemoService>();
#endregion


#region ExprotExcel
builder.Services.AddScoped<ExportExcelService>();



#endregion

builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddRazorPages();

//Install-Package Swashbuckle.AspNetCore
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

//�޸�Ĭ�ϱ��뷢�����5000��5001�����˿�
builder.WebHost.UseUrls(new[] {"http://*:80" });

#region Console
/*http://patorjk.com/software/taag/
_____ ____  __  __ _____         _____ _____ 
/ ____/ __ \|  \/  |  __ \ /\    / ____/ ____|
| |   | |  | | \  / | |__) /  \  | (___| (___  
| |   | |  | | |\/| |  ___/ /\ \  \___ \\___ \ 
| |___| |__| | |  | | |  / ____ \ ____) |___) |
\_____\____/|_|  |_|_| /_/    \_\_____/_____/ 

Author: felix
Version: v3.0.1
Link: https://github.com/felixzhu1989/Compass.Wasm
2022/10/28 10:07:52.282  http server Running on http://10.9.18.31
*/
Console.ForegroundColor= ConsoleColor.Magenta;
Console.WriteLine("   _____ ____  __  __ _____         _____ _____");
Console.WriteLine("  / ____/ __ \\|  \\/  |  __ \\ /\\    / ____/ ____|");
Console.WriteLine(" | |   | |  | | \\  / | |__) /  \\  | (___| (___");
Console.WriteLine(" | |   | |  | | |\\/| |  ___/ /\\ \\  \\___ \\\\___ \\");
Console.WriteLine(" | |___| |__| | |  | | |  / ____ \\ ____) |___) |");
Console.WriteLine("  \\_____\\____/|_|  |_|_| /_/    \\_\\_____/_____/");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("");
Console.WriteLine("Author: felix");
Console.WriteLine("Version: v3.0.1");
Console.WriteLine("Link: https://github.com/felixzhu1989/Compass.Wasm");
Console.WriteLine($"{DateTime.Now} http server Running on http://10.9.18.31");

#endregion

var app = builder.Build();

#region �м��
//������OpenApiҳ��
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseWebAssemblyDebugging();
app.UseDeveloperExceptionPage();

app.UseSwagger();

//app.MapHub<XxxHub>("/Hubs/XxxHub");

//�����¼�
app.UseEventBus();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();//JWT
app.UseAuthorization();

#endregion

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
