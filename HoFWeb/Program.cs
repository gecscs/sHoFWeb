using HoFWeb.Data;
using HoFWeb.Logging;
using HoFWeb.Models;
using HoFWeb.Repositories;
using HoFWeb.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Link the data services settings between the class and the appSettings data
builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("ServiceSettings"));

// Serilog configuration
builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// Custom Serilog logger that uses distinct sincs
builder.Services.AddSingleton<ICustomLogger, CustomLogger>();
// Register Serilog Log Database
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LogDbConnection")));

// Register application database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbConnection")));


// Inject HttpClient to all the services for cleaner usage
builder.Services.AddHttpClient();

// Register external data services
builder.Services.AddScoped<ICreatorStatsService, CreatorStatsService>();
builder.Services.AddScoped<ISingleImageService, SingleImageService>();
builder.Services.AddScoped<ICreatorImagesService, CreatorImagesService>();

builder.Services.AddScoped<IScreenshotItemRepository, ScreenshotItemRepository>();
builder.Services.AddScoped<IScreenshotsProcessor, ScreenshotsProcessor>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Middleware to capture all unexpected errors and log them
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Updates database if Migrations are pending
}

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
