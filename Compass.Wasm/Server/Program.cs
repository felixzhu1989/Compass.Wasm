
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
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console()
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
//redis-cli
//ping
//keys *
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
builder.Services.AddScoped<FSDomainService>();
builder.Services.AddScoped<IFSRepository, FSRepository>();

#endregion

#region IdentityService
//���ݿ⣬DbContext
builder.Services.AddDbContext<IdDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<IdDomainService>();
builder.Services.AddScoped<IIdRepository, IdRepository>();
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
idBuilder.AddEntityFrameworkStores<IdDbContext>().AddDefaultTokenProviders()
    .AddRoleManager<RoleManager<Role>>()
    .AddUserManager<IdUserManager>();
//�����ʼ�
builder.Services.AddScoped<IEmailSender, EmailSender>();
//��ȡ���ķ������е�smtp����
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

#endregion

#region CategoryService
//���ݿ⣬DbContext
builder.Services.AddDbContext<CSDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<CSDomainService>();
builder.Services.AddScoped<ICSRepository, CSRepository>();
#endregion

#region ProjectService
//���ݿ⣬DbContext
builder.Services.AddDbContext<PMDbContext>(options =>
{
    //ָ�����ӵ����ݿ�
    var connStr = builder.Configuration.GetSection("DefaultDB:ConnStr").Value;
    options.UseSqlServer(connStr);
});
builder.Services.AddScoped<PMDomainService>();
builder.Services.AddScoped<IPMRepository, PMRepository>();
#endregion





builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Install-Package Swashbuckle.AspNetCore
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

//�޸�Ĭ�ϱ��뷢�����5000��5001�����˿�
builder.WebHost.UseUrls(new[] {"http://*:80" });


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseWebAssemblyDebugging();

#region �м��

//������OpenApiҳ��
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<ProjectStatusHub>("/Hubs/ProjectStatusHub");


//�����¼�
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
