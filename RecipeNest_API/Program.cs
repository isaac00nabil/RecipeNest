using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RecipeNest_Core.IRepositories;
using RecipeNest_Core.IServices;
using RecipeNest_Core.Models.Context;
using RecipeNests_Infra.Repositories;
using RecipeNests_Infra.Services;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Recipe Nest API",
        Version = "v1",
        Description = "An API for managing recipes and related operations.",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddDbContext<RecipeNestDbContext>(cop =>
cop.UseSqlServer(builder.Configuration.GetValue<string>("sqlconnect")));


builder.Services.AddScoped<IAdminServiceInterface, AdminServiceImplementation>();
builder.Services.AddScoped<IAdminRepositoryInterface, AdminRepositoryImplementation>();

builder.Services.AddScoped<IMemberServiceInterface, MemberServiceImplementation>();
builder.Services.AddScoped<IMemberRepositoryInterface, MemberRepositoryImplementation>();

builder.Services.AddScoped<ISharedServiceInterface, SharedServiceImplementation>();
builder.Services.AddScoped<ISharedRepositoryInterface, SharedRepositoryImplementation>();

var warningLogger = new LoggerConfiguration()
    .WriteTo.File("D:\\Full Stack\\Project\\RecipeNest\\RecipeNest_Core\\Logger\\warningLogger.txt",
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();

var errorLogger = new LoggerConfiguration()
    .WriteTo.File("D:\\Full Stack\\Project\\RecipeNest\\RecipeNest_Core\\Logger\\errorLogger.txt",
    rollingInterval: RollingInterval.Month,
    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
    .CreateLogger();

var informationLogger = new LoggerConfiguration()
    .WriteTo.File("D:\\Full Stack\\Project\\RecipeNest\\RecipeNest_Core\\Logger\\informationLogger.txt",
    rollingInterval: RollingInterval.Hour,
    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();

var debugLogger = new LoggerConfiguration()
    .WriteTo.File("D:\\Full Stack\\Project\\RecipeNest\\RecipeNest_Core\\Logger\\debugLogger.txt",
    rollingInterval: RollingInterval.Minute,
    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
    .CreateLogger();

warningLogger.Warning("This is a warning log message");
errorLogger.Error("This is an error log message");
informationLogger.Information("This is an information log message");
debugLogger.Debug("This is a debug log message");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
