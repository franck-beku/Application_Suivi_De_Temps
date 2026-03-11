using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.Alerts;

public interface IAlertService
{
    /// <summary>
    /// Vérifier et générer automatiquement toutes les alertes pour aujourd'hui
    /// </summary>
    Task<List<AlertLog>> CheckAndGenerateAlertsAsync(int clientId);
    
    /// <summary>
    /// Obtenir les alertes récentes
    /// </summary>
    Task<List<AlertLog>> GetRecentAlertsAsync(int clientId, int count = 10);
    
    /// <summary>
    /// Obtenir les alertes non lues
    /// </summary>
    Task<List<AlertLog>> GetUnreadAlertsAsync(int clientId);
    
    /// <summary>
    /// Marquer toutes les alertes comme lues
    /// </summary>
    Task MarkAllAsReadAsync(int clientId);
    
    /// <summary>
    /// Supprimer toutes les alertes
    /// </summary>
    Task ClearAllAlertsAsync(int clientId);
}