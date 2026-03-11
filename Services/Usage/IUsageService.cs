using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.Usage;

public interface IUsageService
{
    /// <summary>
    /// Enregistrer du temps d'utilisation pour une app
    /// </summary>
    Task<UsageEntry> RecordUsageAsync(int clientId, int appId, int minutes, DateOnly? date = null);
    
    /// <summary>
    /// Obtenir le résumé d'usage pour une date
    /// </summary>
    Task<UsageSummary> GetDailySummaryAsync(int clientId, DateOnly date);
    
    /// <summary>
    /// Obtenir le résumé d'usage pour aujourd'hui
    /// </summary>
    Task<UsageSummary> GetTodaySummaryAsync(int clientId);
    
    /// <summary>
    /// Obtenir le temps total utilisé aujourd'hui
    /// </summary>
    Task<int> GetTodayTotalMinutesAsync(int clientId);
    
    /// <summary>
    /// Obtenir le temps total pour une période
    /// </summary>
    Task<int> GetPeriodTotalMinutesAsync(int clientId, DateOnly startDate, DateOnly endDate);
    
    /// <summary>
    /// Obtenir toutes les entrées pour une période
    /// </summary>
    Task<List<UsageEntry>> GetUsageEntriesAsync(int clientId, DateOnly startDate, DateOnly endDate);
    
    /// <summary>
    /// Réinitialiser toutes les données d'usage
    /// </summary>
    Task ResetAllUsageAsync(int clientId);
}