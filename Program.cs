


/*
using Microsoft.EntityFrameworkCore;
using Application_Suivi_De_Temps.Data;
                                                                                                      
var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run(); */

using Microsoft.EntityFrameworkCore;
using Application_Suivi_De_Temps.Data;
using Application_Suivi_De_Temps.Data.Seed;
using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Data.Repositories.Implementations;
using Application_Suivi_De_Temps.Services.Usage;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// DI - Repositories & Services (fonctionnalité #1)
builder.Services.AddScoped<IUsageRepository, UsageRepository>();
builder.Services.AddScoped<IUsageService, UsageService>();

var app = builder.Build();

//  Seed DB (création + données simulées)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
