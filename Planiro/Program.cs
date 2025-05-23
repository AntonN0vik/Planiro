using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Planiro.Infrastructure.Data.Configurations;

var builder = WebApplication.CreateBuilder(args);
// Добавляем поддержку статических файлов SPA
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend",
        policy => {
            policy.WithOrigins("http://localhost:3000") // Адрес вашего React-приложения
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});



builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "planiro/build";
});
// Конфигурация базы данных PostgreSQL
builder.Services.AddDbContext<PlaniroDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
    // Для логирования SQL-запросов (опционально)
    options.UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

// Настройка React
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "planiro";
    
    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
    }
});


app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();