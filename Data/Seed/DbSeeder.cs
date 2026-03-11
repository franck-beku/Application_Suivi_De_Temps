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

        // 2) UserPreferences
        if (!await db.UserPreferences.AnyAsync(p => p.ClientId == client.Id))
        {
            db.UserPreferences.Add(new UserPreferences
            {
                ClientId = client.Id,
                DefaultDailyGoalMinutes = 180,
                EnableNotifications = true,
                EnableBreakReminders = true,
                EnableWeeklyReport = true,
                DisconnectStartTime = new TimeOnly(22, 0),
                DisconnectEndTime = new TimeOnly(7, 0),
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

        // 5) UsageEntries
        if (!await db.UsageEntries.AnyAsync())
        {
            var apps = await db.Apps.ToListAsync();
            var rng = new Random();
            for (int i = 0; i < 7; i++)
            {
                var date = DateOnly.FromDateTime(DateTime.Today.AddDays(-i));
                foreach (var app in apps)
                {
                    int minutes = rng.Next(0, 91);
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

        // 6) CategoryThresholds
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
                    ThresholdMinutes = 90,
                    Type = ThresholdType.Daily,
                    IsActive = true
                });
            }

            if (gamesCat != null)
            {
                db.CategoryThresholds.Add(new CategoryThreshold
                {
                    ClientId = client.Id,
                    CategoryId = gamesCat.Id,
                    ThresholdMinutes = 45,
                    Type = ThresholdType.Daily,
                    IsActive = true
                });
            }

            await db.SaveChangesAsync();
        }

        // 7) BreakActivities
        if (!await db.BreakActivities.AnyAsync())
        {
            db.BreakActivities.AddRange(
                new BreakActivity
                {
                    Name = "Marche rapide",
                    Description = "Faites une courte marche à l'extérieur pour vous aérer",
                    Category = BreakCategory.Physical,
                    SuggestedDurationMinutes = 10,
                    IconClass = "bi-person-walking"
                },
                new BreakActivity
                {
                    Name = "Étirements",
                    Description = "Quelques étirements pour détendre vos muscles",
                    Category = BreakCategory.Physical,
                    SuggestedDurationMinutes = 5,
                    IconClass = "bi-person-arms-up"
                },
                new BreakActivity
                {
                    Name = "Yoga",
                    Description = "Quelques postures de yoga pour se recentrer",
                    Category = BreakCategory.Physical,
                    SuggestedDurationMinutes = 15,
                    IconClass = "bi-heart-pulse"
                },
                new BreakActivity
                {
                    Name = "Méditation",
                    Description = "Méditation guidée ou respiration consciente",
                    Category = BreakCategory.Mental,
                    SuggestedDurationMinutes = 10,
                    IconClass = "bi-peace"
                },
                new BreakActivity
                {
                    Name = "Exercices de respiration",
                    Description = "Respirez profondément pour vous détendre",
                    Category = BreakCategory.Mental,
                    SuggestedDurationMinutes = 5,
                    IconClass = "bi-wind"
                },
                new BreakActivity
                {
                    Name = "Dessin/Coloriage",
                    Description = "Dessinez ou coloriez quelque chose qui vous inspire",
                    Category = BreakCategory.Creative,
                    SuggestedDurationMinutes = 15,
                    IconClass = "bi-palette"
                },
                new BreakActivity
                {
                    Name = "Musique",
                    Description = "Jouez d'un instrument ou écoutez votre musique préférée",
                    Category = BreakCategory.Creative,
                    SuggestedDurationMinutes = 10,
                    IconClass = "bi-music-note"
                },
                new BreakActivity
                {
                    Name = "Appeler un ami",
                    Description = "Prenez des nouvelles d'un proche",
                    Category = BreakCategory.Social,
                    SuggestedDurationMinutes = 10,
                    IconClass = "bi-telephone"
                },
                new BreakActivity
                {
                    Name = "Jeu de société",
                    Description = "Jouez à un jeu avec votre famille",
                    Category = BreakCategory.Social,
                    SuggestedDurationMinutes = 20,
                    IconClass = "bi-puzzle"
                },
                new BreakActivity
                {
                    Name = "Lecture",
                    Description = "Lisez quelques pages d'un livre",
                    Category = BreakCategory.Relaxation,
                    SuggestedDurationMinutes = 15,
                    IconClass = "bi-book"
                },
                new BreakActivity
                {
                    Name = "Observer la nature",
                    Description = "Regardez par la fenêtre ou sortez observer la nature",
                    Category = BreakCategory.Relaxation,
                    SuggestedDurationMinutes = 5,
                    IconClass = "bi-tree"
                },
                new BreakActivity
                {
                    Name = "Caresser un animal",
                    Description = "Passez du temps avec votre animal de compagnie",
                    Category = BreakCategory.Relaxation,
                    SuggestedDurationMinutes = 10,
                    IconClass = "bi-heart"
                },
                new BreakActivity
                {
                    Name = "Ranger sa chambre",
                    Description = "Rangez votre espace pour clarifier votre esprit",
                    Category = BreakCategory.Productive,
                    SuggestedDurationMinutes = 10,
                    IconClass = "bi-house"
                },
                new BreakActivity
                {
                    Name = "Préparer une collation",
                    Description = "Préparez un en-cas sain",
                    Category = BreakCategory.Productive,
                    SuggestedDurationMinutes = 10,
                    IconClass = "bi-cup-straw"
                }
            );
            await db.SaveChangesAsync();
        }

        // 8) Badges
        if (!await db.Badges.AnyAsync())
        {
            db.Badges.AddRange(
                new Badge
                {
                    Name = "Premier pas",
                    Description = "Respecter ton objectif quotidien pour la première fois",
                    Type = BadgeType.TimeManagement,
                    IconClass = "bi-star",
                    Color = "#28a745",
                    RequiredValue = 1,
                    UnlockCriteria = "1 jour sous l'objectif"
                },
                new Badge
                {
                    Name = "Semaine parfaite",
                    Description = "7 jours consécutifs sous ton objectif quotidien",
                    Type = BadgeType.Consistency,
                    IconClass = "bi-calendar-check",
                    Color = "#17a2b8",
                    RequiredValue = 7,
                    UnlockCriteria = "7 jours consécutifs"
                },
                new Badge
                {
                    Name = "Pause bien-être",
                    Description = "Prendre ta première pause numérique",
                    Type = BadgeType.BreakMaster,
                    IconClass = "bi-heart",
                    Color = "#e83e8c",
                    RequiredValue = 1,
                    UnlockCriteria = "1 pause enregistrée"
                },
                new Badge
                {
                    Name = "Maître des pauses",
                    Description = "Enregistrer 20 pauses numériques",
                    Type = BadgeType.BreakMaster,
                    IconClass = "bi-gem",
                    Color = "#6f42c1",
                    RequiredValue = 20,
                    UnlockCriteria = "20 pauses"
                },
                new Badge
                {
                    Name = "Nuit paisible",
                    Description = "Utiliser le mode déconnexion pendant 7 nuits",
                    Type = BadgeType.Disconnect,
                    IconClass = "bi-moon-stars",
                    Color = "#6610f2",
                    RequiredValue = 7,
                    UnlockCriteria = "7 nuits en mode déconnexion"
                },
                new Badge
                {
                    Name = "Une semaine",
                    Description = "Utiliser l'app pendant 7 jours",
                    Type = BadgeType.Milestone,
                    IconClass = "bi-trophy",
                    Color = "#fd7e14",
                    RequiredValue = 7,
                    UnlockCriteria = "7 jours d'utilisation"
                }
            );
            await db.SaveChangesAsync();
        }

        // 9) Tips
        if (!await db.Tips.AnyAsync())
        {
            db.Tips.AddRange(
                new Tip
                {
                    Title = "Règle 20-20-20 pour vos yeux",
                    Content = "Toutes les 20 minutes, regardez quelque chose à 20 pieds (6 mètres) pendant 20 secondes pour reposer vos yeux.",
                    Category = TipCategory.Health,
                    IconClass = "bi-eye"
                },
                new Tip
                {
                    Title = "Adoptez une bonne posture",
                    Content = "Gardez votre dos droit, vos épaules détendues et votre écran à hauteur des yeux pour éviter les douleurs.",
                    Category = TipCategory.Health,
                    IconClass = "bi-person-standing"
                },
                new Tip
                {
                    Title = "Évitez les écrans avant de dormir",
                    Content = "Arrêtez d'utiliser vos écrans au moins 1 heure avant le coucher pour améliorer la qualité de votre sommeil.",
                    Category = TipCategory.Health,
                    IconClass = "bi-moon-stars"
                },
                new Tip
                {
                    Title = "Technique Pomodoro",
                    Content = "Travaillez pendant 25 minutes, puis prenez une pause de 5 minutes. Répétez 4 fois, puis faites une pause plus longue.",
                    Category = TipCategory.Productivity,
                    IconClass = "bi-clock"
                },
                new Tip
                {
                    Title = "Désactivez les notifications",
                    Content = "Coupez les notifications non essentielles pour rester concentré sur vos tâches importantes.",
                    Category = TipCategory.Productivity,
                    IconClass = "bi-bell-slash"
                },
                new Tip
                {
                    Title = "Une tâche à la fois",
                    Content = "Le multitâche diminue votre productivité. Concentrez-vous sur une seule tâche pour être plus efficace.",
                    Category = TipCategory.Productivity,
                    IconClass = "bi-list-check"
                },
                new Tip
                {
                    Title = "Pratiquez la pleine conscience",
                    Content = "Prenez quelques minutes par jour pour méditer ou simplement respirer profondément et être présent.",
                    Category = TipCategory.WellBeing,
                    IconClass = "bi-peace"
                },
                new Tip
                {
                    Title = "Sortez prendre l'air",
                    Content = "Passez du temps à l'extérieur chaque jour. La nature et la lumière naturelle améliorent votre humeur.",
                    Category = TipCategory.WellBeing,
                    IconClass = "bi-tree"
                },
                new Tip
                {
                    Title = "Connectez-vous aux autres",
                    Content = "Privilégiez les interactions en face-à-face plutôt que virtuelles pour renforcer vos relations.",
                    Category = TipCategory.WellBeing,
                    IconClass = "bi-people"
                },
                new Tip
                {
                    Title = "Définissez des limites",
                    Content = "Fixez-vous des plages horaires sans écran, comme pendant les repas ou avant le coucher.",
                    Category = TipCategory.Habits,
                    IconClass = "bi-calendar-x"
                },
                new Tip
                {
                    Title = "Créez une routine matinale",
                    Content = "Commencez votre journée sans écran : petit-déjeuner, exercice ou lecture avant de consulter votre téléphone.",
                    Category = TipCategory.Habits,
                    IconClass = "bi-sunrise"
                },
                new Tip
                {
                    Title = "Tenez un journal",
                    Content = "Notez vos émotions et habitudes numériques pour mieux comprendre votre relation avec la technologie.",
                    Category = TipCategory.Habits,
                    IconClass = "bi-journal-text"
                },
                new Tip
                {
                    Title = "Mode avion en soirée",
                    Content = "Activez le mode avion après une certaine heure pour éviter les distractions nocturnes.",
                    Category = TipCategory.Technology,
                    IconClass = "bi-airplane"
                },
                new Tip
                {
                    Title = "Organisez vos applications",
                    Content = "Rangez les apps distrayantes dans un dossier ou sur un écran éloigné pour réduire leur utilisation impulsive.",
                    Category = TipCategory.Technology,
                    IconClass = "bi-folder"
                },
                new Tip
                {
                    Title = "Utilisez le mode sombre",
                    Content = "Le mode sombre réduit la fatigue oculaire, surtout lors d'utilisation prolongée en soirée.",
                    Category = TipCategory.Technology,
                    IconClass = "bi-moon"
                }
            );
            await db.SaveChangesAsync();
        }
    }
}