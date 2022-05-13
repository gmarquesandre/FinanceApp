
using FinancialApi.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// Add services to the container.
builder.Services.AddDbContext<FinanceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinancialAPI", Version = "v1" });
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

app.UseAuthorization();

app.MapControllers();

app.Run();
