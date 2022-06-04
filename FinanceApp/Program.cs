using FinanceApp.Core.Services.CrudServices;
using FinanceApp.Core.Services.CrudServices.Implementations;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Core.Services.ForecastServices.Implementations;
using FinanceApp.Core.Services.UserServices;
using FinanceApp.Core.Services.UserServices.Interfaces;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddMemoryCache();

builder.Services.AddTransient<ITreasuryBondService, TreasuryBondService>();
builder.Services.AddTransient<IPrivateFixedIncomeService, PrivateFixedIncomeService>();
builder.Services.AddTransient<IIncomeService, IncomeService>();
builder.Services.AddTransient<ISpendingService, SpendingService>();
builder.Services.AddTransient<ISpendingForecast, SpendingForecast>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICurrentBalanceService, CurrentBalanceService>();
builder.Services.AddTransient<IFGTSService, FGTSService>();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserRegisterService, UserRegisterService>();
builder.Services.AddTransient<IForecastService, ForecastService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);



// Add services to the container.
builder.Services.AddDbContext<FinanceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

});
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

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
