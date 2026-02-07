using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        // 1) Client
        if (!await db.Clients.AnyAsync())
        {
            db.Clients.Add(new Client { DisplayName = "Adolescent" });
            await db.SaveChangesAsync();
        }

        var client = await db.Clients.FirstAsync();

        // 2) UserPreferences (si n'existe pas)
        if (!await db.UserPreferences.AnyAsync(p => p.ClientId == client.Id))
        {
            db.UserPreferences.Add(new UserPreferences
            {
                ClientId = client.Id,
                DefaultDailyGoalMinutes = 180, // 3h
                EnableNotifications = true,
                EnableBreakReminders = true,
                EnableWeeklyReport = true,
                DisconnectStartTime = new TimeOnly(22, 0), // 22:00
                DisconnectEndTime = new TimeOnly(7, 0),    // 07:00
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        // 3) Categories
        if (!await db.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() { Type = AppCategory.Social, Name = "Réseaux sociaux" },
                new() { Type = AppCategory.Video, Name = "Vidéo" },
                new() { Type = AppCategory.Games, Name = "Jeux" },
                new() { Type = AppCategory.Study, Name = "Études" },
                new() { Type = AppCategory.Messaging, Name = "Messagerie" },
                new() { Type = AppCategory.Other, Name = "Autre" }
            };
            db.Categories.AddRange(categories);
            await db.SaveChangesAsync();
        }

        // 4) Apps
        if (!await db.Apps.AnyAsync())
        {
            var cat = await db.Categories.ToListAsync();
            int social = cat.Single(c => c.Type == AppCategory.Social).Id;
            int video = cat.Single(c => c.Type == AppCategory.Video).Id;
            int games = cat.Single(c => c.Type == AppCategory.Games).Id;
            int study = cat.Single(c => c.Type == AppCategory.Study).Id;
            int msg = cat.Single(c => c.Type == AppCategory.Messaging).Id;

            db.Apps.AddRange(
                new App { Name = "Instagram", CategoryId = social },
                new App { Name = "TikTok", CategoryId = social },
                new App { Name = "YouTube", CategoryId = video },
                new App { Name = "Snapchat", CategoryId = msg },
                new App { Name = "WhatsApp", CategoryId = msg },
                new App { Name = "Minecraft", CategoryId = games },
                new App { Name = "Google Classroom", CategoryId = study }
            );
            await db.SaveChangesAsync();
        }

        // 5) Usage (7 derniers jours) — simulé
        if (!await db.UsageEntries.AnyAsync())
        {
            var apps = await db.Apps.ToListAsync();
            var rng = new Random();
            for (int i = 0; i < 7; i++)
            {
                var date = DateOnly.FromDateTime(DateTime.Today.AddDays(-i));
                foreach (var app in apps)
                {
                    // minutes simulées (0 à 90)
                    int minutes = rng.Next(0, 91);
                    // un peu plus de minutes pour social/video
                    if (app.Name is "Instagram" or "TikTok" or "YouTube")
                        minutes += rng.Next(10, 61);

                    db.UsageEntries.Add(new UsageEntry
                    {
                        ClientId = client.Id,
                        AppId = app.Id,
                        Date = date,
                        Minutes = minutes
                    });
                }
            }
            await db.SaveChangesAsync();
        }

        // 6) Seuils par défaut (CategoryThreshold)
        if (!await db.CategoryThresholds.AnyAsync())
        {
            var categories = await db.Categories.ToListAsync();
            var socialCat = categories.FirstOrDefault(c => c.Type == AppCategory.Social);
            var gamesCat = categories.FirstOrDefault(c => c.Type == AppCategory.Games);

            if (socialCat != null)
            {
                db.CategoryThresholds.Add(new CategoryThreshold
                {
                    ClientId = client.Id,
                    CategoryId = socialCat.Id,
                    ThresholdMinutes = 90, // 1h30 pour réseaux sociaux
                    Type = Models.Enums.ThresholdType.Daily,
                    IsActive = true
                });
            }

            if (gamesCat != null)
            {
                db.CategoryThresholds.Add(new CategoryThreshold
                {
                    ClientId = client.Id,
                    CategoryId = gamesCat.Id,
                    ThresholdMinutes = 45, // 45min pour jeux
                    Type = Models.Enums.ThresholdType.Daily,
                    IsActive = true
                });
            }

            await db.SaveChangesAsync();
        }
    }
}