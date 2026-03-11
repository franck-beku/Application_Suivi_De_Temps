using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Services.Usage;
using Application_Suivi_De_Temps.Services.Profile;
using Application_Suivi_De_Temps.Services.Disconnect;
using Application_Suivi_De_Temps.Services.Alerts;

namespace Application_Suivi_De_Temps.Services.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IUsageService _usageService;
    private readonly IProfileService _profileService;
    private readonly IDisconnectService _disconnectService;
    private readonly IAlertService _alertService; // ← AJOUTÉ pour UC6

    public DashboardService(
        IUsageService usageService, 
        IProfileService profileService,
        IDisconnectService disconnectService,
        IAlertService alertService) // ← AJOUTÉ pour UC6
    {
        _usageService = usageService;
        _profileService = profileService;
        _disconnectService = disconnectService;
        _alertService = alertService; // ← AJOUTÉ pour UC6
    }

    public async Task<DashboardData> GetDashboardDataAsync(int clientId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        // 1. Obtenir les préférences pour l'objectif quotidien
        var preferences = await _profileService.GetPreferencesAsync(clientId);
        var dailyGoal = preferences?.DefaultDailyGoalMinutes ?? 180; // 3h par défaut
        
        // 2. Obtenir le résumé d'aujourd'hui
        var todaySummary = await _usageService.GetTodaySummaryAsync(clientId);
        var todayMinutes = todaySummary.TotalMinutes;
        
        // 3. Calculer la progression
        var progressPercentage = dailyGoal > 0 
            ? (int)((todayMinutes / (double)dailyGoal) * 100) 
            : 0;
        
        var isOverGoal = todayMinutes > dailyGoal;
        var minutesOverGoal = isOverGoal ? todayMinutes - dailyGoal : 0;
        
        // 4. Calculer les statistiques hebdomadaires (7 derniers jours)
        var sevenDaysAgo = today.AddDays(-6); // Aujourd'hui + 6 jours précédents = 7 jours
        var weeklyTotal = await _usageService.GetPeriodTotalMinutesAsync(clientId, sevenDaysAgo, today);
        var weeklyAverage = weeklyTotal / 7;
        
        // 5. Calculer la variation hebdomadaire (par rapport à la semaine précédente)
        var fourteenDaysAgo = today.AddDays(-13);
        var previousWeekEnd = today.AddDays(-7);
        var previousWeekTotal = await _usageService.GetPeriodTotalMinutesAsync(clientId, fourteenDaysAgo, previousWeekEnd);
        var previousWeekAverage = previousWeekTotal > 0 ? previousWeekTotal / 7 : 1;
        
        var weeklyVariation = previousWeekAverage > 0
            ? (int)(((weeklyAverage - previousWeekAverage) / (double)previousWeekAverage) * 100)
            : 0;
        
        var isWeeklyIncreasing = weeklyVariation > 0;
        
        // 6. Top 4 applications d'aujourd'hui
        var topApps = todaySummary.AppUsages
            .Take(4)
            .Select(a => new TopApp
            {
                AppId = a.AppId,
                AppName = a.AppName,
                CategoryName = a.CategoryName,
                Minutes = a.Minutes,
                Percentage = a.Percentage
            })
            .ToList();
        
        // 7. État de déconnexion
        var disconnectStatus = await _disconnectService.GetDisconnectStatusAsync(clientId);
        
        // 8. Vérifier et générer les alertes (UC6) ← AJOUTÉ
        await _alertService.CheckAndGenerateAlertsAsync(clientId);
        var unreadAlerts = await _alertService.GetUnreadAlertsAsync(clientId);
        
        // 9. Suggestion d'activité de pause si mode déconnexion actif (UC8)
        BreakActivity? suggestedActivity = null;
        if (disconnectStatus?.IsInDisconnectPeriod == true)
        {
            suggestedActivity = await _disconnectService.GetSuggestedBreakActivityAsync();
        }
        
        // 10. Construire le dashboard
        return new DashboardData
        {
            TodayMinutes = todayMinutes,
            DailyGoalMinutes = dailyGoal,
            ProgressPercentage = progressPercentage,
            IsOverGoal = isOverGoal,
            MinutesOverGoal = minutesOverGoal,
            
            WeeklyAverageMinutes = weeklyAverage,
            WeeklyTotalMinutes = weeklyTotal,
            WeeklyVariationPercentage = Math.Abs(weeklyVariation),
            IsWeeklyIncreasing = isWeeklyIncreasing,
            
            TopApps = topApps,
            DisconnectStatus = disconnectStatus,
            UnreadAlerts = unreadAlerts, // ← AJOUTÉ pour UC6
            SuggestedBreakActivity = suggestedActivity // ← AJOUTÉ pour UC8
        };
    }
}