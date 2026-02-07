using Application_Suivi_De_Temps.Data;
using Application_Suivi_De_Temps.Data.Repositories.Implementations;
using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Data.Seed;
using Application_Suivi_De_Temps.Services.Profile;
using Application_Suivi_De_Temps.Services.Threshold;
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

// Enregistrement des Services
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IThresholdService, ThresholdService>();

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

app.MapRazorPages();

app.Run();