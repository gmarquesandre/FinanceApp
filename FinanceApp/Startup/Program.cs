using FinanceApp.Api.Startup;
using FinanceApp.Shared.Models.CommonTables;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FinanceApp.EntityFramework;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddMemoryCache();

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var connectionStringHangfire = builder.Configuration.GetConnectionString("HangfireConnection");
builder.Services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionStringHangfire, new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true
    }));
builder.Services.AddHangfireServer(configuration =>
{
    configuration.Queues = new[] { "default", "asset" };
    configuration.WorkerCount = 20;
    configuration.ServerName = "Default Server";
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddMvc();

// Add services to the container.
builder.Services.AddDbContext<FinanceContext>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
         //,ServiceLifetime.Transient
         );

builder.Services.AddIdentity<CustomIdentityUser, IdentityRole<int>>(opt =>
{
    //opt.SignIn.RequireConfirmedEmail = true;
    opt.User.RequireUniqueEmail = true;
})
    .AddSignInManager()

    .AddEntityFrameworkStores<FinanceContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
           .AddJwtBearer(token =>
           {
               token.RequireHttpsMetadata = false;
               token.SaveToken = true;
               token.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0asdjas09djsa09djasdjsadajsd09asjd09sajcnzxn")),
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ClockSkew = TimeSpan.Zero,
               };
           });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinancialAPI", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});
builder.Services.AddCors();





var app = builder.Build();

app.UseCors(options =>
               options.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
app.UseHangfireDashboard();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Services.AddDefaultJobs();
app.Run();
