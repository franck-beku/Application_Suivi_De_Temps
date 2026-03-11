using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Services.Breaks;

public interface IBreakService
{
    /// <summary>
    /// Obtenir toutes les activités de pause disponibles
    /// </summary>
    Task<List<BreakActivity>> GetAllActivitiesAsync();
    
    /// <summary>
    /// Obtenir les activités par catégorie
    /// </summary>
    Task<List<BreakActivity>> GetActivitiesByCategoryAsync(BreakCategory category);
    
    /// <summary>
    /// Enregistrer une session de pause
    /// </summary>
    Task<BreakSession> RecordBreakSessionAsync(int clientId, int activityId, int durationMinutes, string? notes = null);
    
    /// <summary>
    /// Obtenir les sessions de pause du jour
    /// </summary>
    Task<List<BreakSession>> GetTodaySessionsAsync(int clientId);
    
    /// <summary>
    /// Obtenir les statistiques de pause
    /// </summary>
    Task<BreakStats> GetBreakStatsAsync(int clientId);
    
    /// <summary>
    /// Supprimer toutes les sessions de pause
    /// </summary>
    Task DeleteAllSessionsAsync(int clientId);
}