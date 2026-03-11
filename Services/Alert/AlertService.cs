using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;
using Application_Suivi_De_Temps.Services.Usage;
using Application_Suivi_De_Temps.Services.Profile;
using Application_Suivi_De_Temps.Services.Threshold;

namespace Application_Suivi_De_Temps.Services.Alerts;

public class AlertService : IAlertService
{
    private readonly IAlertRepository _alertRepo;
    private readonly IUsageService _usageService;
    private readonly IProfileService _profileService;
    private readonly IThresholdService _thresholdService;

    public AlertService(
        IAlertRepository alertRepo,
        IUsageService usageService,
        IProfileService profileService,
        IThresholdService thresholdService)
    {
        _alertRepo = alertRepo;
        _usageService = usageService;
        _profileService = profileService;
        _thresholdService = thresholdService;
    }

    public async Task<List<AlertLog>> CheckAndGenerateAlertsAsync(int clientId)
    {
        var generatedAlerts = new List<AlertLog>();
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        // Récupérer les alertes déjà générées aujourd'hui pour éviter les doublons
        var existingAlerts = await _alertRepo.GetAlertsAsync(clientId, today);

        // 1. Vérifier l'objectif quotidien global
        var dailyGoalAlert = await CheckDailyGoalAsync(clientId, existingAlerts);
        if (dailyGoalAlert != null)
        {
            generatedAlerts.Add(dailyGoalAlert);
        }

        // 2. Vérifier les seuils par application
        var appAlerts = await CheckAppThresholdsAsync(clientId, existingAlerts);
        generatedAlerts.AddRange(appAlerts);

        // 3. Vérifier les seuils par catégorie
        var categoryAlerts = await CheckCategoryThresholdsAsync(clientId, existingAlerts);
        generatedAlerts.AddRange(categoryAlerts);

        return generatedAlerts;
    }

    private async Task<AlertLog?> CheckDailyGoalAsync(int clientId, List<AlertLog> existingAlerts)
    {
        // Vérifier si une alerte d'objectif quotidien existe déjà aujourd'hui
        if (existingAlerts.Any(a => a.Type == AlertType.DailyGoalExceeded))
        {
            return null;
        }

        var preferences = await _profileService.GetPreferencesAsync(clientId);
        if (preferences == null || !preferences.EnableNotifications)
        {
            return null;
        }

        var dailyGoal = preferences.DefaultDailyGoalMinutes;
        var todayMinutes = await _usageService.GetTodayTotalMinutesAsync(clientId);

        if (todayMinutes > dailyGoal)
        {
            var excess = todayMinutes - dailyGoal;
            var severity = CalculateSeverity(todayMinutes, dailyGoal);

            var alert = new AlertLog
            {
                ClientId = clientId,
                Type = AlertType.DailyGoalExceeded,
                Severity = severity,
                Title = "Objectif quotidien dépassé",
                Message = $"Vous avez dépassé votre objectif quotidien de {FormatMinutes(dailyGoal)}.",
                CurrentMinutes = todayMinutes,
                ThresholdMinutes = dailyGoal,
                ExcessMinutes = excess
            };

            return await _alertRepo.CreateAlertAsync(alert);
        }

        return null;
    }

    private async Task<List<AlertLog>> CheckAppThresholdsAsync(int clientId, List<AlertLog> existingAlerts)
    {
        var alerts = new List<AlertLog>();
        var appThresholds = await _thresholdService.GetAppThresholdsAsync(clientId);
        var todaySummary = await _usageService.GetTodaySummaryAsync(clientId);

        foreach (var threshold in appThresholds)
        {
            // Vérifier si alerte déjà générée
            if (existingAlerts.Any(a => a.Type == AlertType.AppThresholdExceeded && a.AppId == threshold.AppId))
            {
                continue;
            }

            var appUsage = todaySummary.AppUsages.FirstOrDefault(a => a.AppId == threshold.AppId);
            if (appUsage != null && appUsage.Minutes > threshold.ThresholdMinutes)
            {
                var excess = appUsage.Minutes - threshold.ThresholdMinutes;
                var severity = CalculateSeverity(appUsage.Minutes, threshold.ThresholdMinutes);

                var alert = new AlertLog
                {
                    ClientId = clientId,
                    Type = AlertType.AppThresholdExceeded,
                    Severity = severity,
                    AppId = threshold.AppId,
                    Title = $"Seuil dépassé : {appUsage.AppName}",
                    Message = $"Vous avez dépassé votre seuil de {FormatMinutes(threshold.ThresholdMinutes)} pour {appUsage.AppName}.",
                    CurrentMinutes = appUsage.Minutes,
                    ThresholdMinutes = threshold.ThresholdMinutes,
                    ExcessMinutes = excess
                };

                var created = await _alertRepo.CreateAlertAsync(alert);
                alerts.Add(created);
            }
        }

        return alerts;
    }

    private async Task<List<AlertLog>> CheckCategoryThresholdsAsync(int clientId, List<AlertLog> existingAlerts)
    {
        var alerts = new List<AlertLog>();
        var categoryThresholds = await _thresholdService.GetCategoryThresholdsAsync(clientId);
        var todaySummary = await _usageService.GetTodaySummaryAsync(clientId);

        foreach (var threshold in categoryThresholds)
        {
            // Vérifier si alerte déjà générée
            if (existingAlerts.Any(a => a.Type == AlertType.CategoryThresholdExceeded && a.CategoryId == threshold.CategoryId))
            {
                continue;
            }

            var categoryUsage = todaySummary.CategoryUsages.FirstOrDefault(c => c.CategoryId == threshold.CategoryId);
            if (categoryUsage != null && categoryUsage.Minutes > threshold.ThresholdMinutes)
            {
                var excess = categoryUsage.Minutes - threshold.ThresholdMinutes;
                var severity = CalculateSeverity(categoryUsage.Minutes, threshold.ThresholdMinutes);

                var alert = new AlertLog
                {
                    ClientId = clientId,
                    Type = AlertType.CategoryThresholdExceeded,
                    Severity = severity,
                    CategoryId = threshold.CategoryId,
                    Title = $"Seuil dépassé : {categoryUsage.CategoryName}",
                    Message = $"Vous avez dépassé votre seuil de {FormatMinutes(threshold.ThresholdMinutes)} pour la catégorie {categoryUsage.CategoryName}.",
                    CurrentMinutes = categoryUsage.Minutes,
                    ThresholdMinutes = threshold.ThresholdMinutes,
                    ExcessMinutes = excess
                };

                var created = await _alertRepo.CreateAlertAsync(alert);
                alerts.Add(created);
            }
        }

        return alerts;
    }

    public async Task<List<AlertLog>> GetRecentAlertsAsync(int clientId, int count = 10)
    {
        return await _alertRepo.GetRecentAlertsAsync(clientId, count);
    }

    public async Task<List<AlertLog>> GetUnreadAlertsAsync(int clientId)
    {
        return await _alertRepo.GetUnreadAlertsAsync(clientId);
    }

    public async Task MarkAllAsReadAsync(int clientId)
    {
        await _alertRepo.MarkAllAsReadAsync(clientId);
    }

    public async Task ClearAllAlertsAsync(int clientId)
    {
        await _alertRepo.DeleteAllAlertsAsync(clientId);
    }

    private AlertSeverity CalculateSeverity(int current, int threshold)
    {
        var percentage = (double)current / threshold * 100;
        
        if (percentage >= 150) return AlertSeverity.Critical; // 150%+
        if (percentage >= 120) return AlertSeverity.Warning;  // 120-149%
        return AlertSeverity.Info;                            // 100-119%
    }

    private string FormatMinutes(int minutes)
    {
        var hours = minutes / 60;
        var mins = minutes % 60;
        return mins > 0 ? $"{hours}h {mins}m" : $"{hours}h";
    }
}