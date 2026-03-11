using Application_Suivi_De_Temps.Data;
using Application_Suivi_De_Temps.Data.Repositories.Implementations;
using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Data.Seed;
using Application_Suivi_De_Temps.Services.Profile;
using Application_Suivi_De_Temps.Services.Threshold;
using Application_Suivi_De_Temps.Services.Disconnect;
using Application_Suivi_De_Temps.Services.Usage;
using Application_Suivi_De_Temps.Services.Dashboard;
using Application_Suivi_De_Temps.Services.Alerts;
using Application_Suivi_De_Temps.Services.Breaks;
using Application_Suivi_De_Temps.Services.AppBlocks;
using Application_Suivi_De_Temps.Services.Goals;
using Application_Suivi_De_Temps.Services.Badges;
using Application_Suivi_De_Temps.Services.Tips;
using Application_Suivi_De_Temps.Services.DataManagement;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Configuration de la base de données
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enregistrement des Repositories
builder.Services.AddScoped<IUserPreferencesRepository, UserPreferencesRepository>();
builder.Services.AddScoped<IThresholdRepository, ThresholdRepository>();
builder.Services.AddScoped<IUsageRepository, UsageRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IBreakRepository, BreakRepository>();
builder.Services.AddScoped<IAppBlockRepository, AppBlockRepository>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();

// Enregistrement des Services
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IThresholdService, ThresholdService>();
builder.Services.AddScoped<IDisconnectService, DisconnectService>();
builder.Services.AddScoped<IUsageService, UsageService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IBreakService, BreakService>();
builder.Services.AddScoped<IAppBlockService, AppBlockService>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<ITipService, TipService>();
builder.Services.AddScoped<IDataManagementService, DataManagementService>();

var app = builder.Build();

// Seed de la base de données
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Redirection automatique de la racine vers Dashboard


app.MapRazorPages();

app.Run();