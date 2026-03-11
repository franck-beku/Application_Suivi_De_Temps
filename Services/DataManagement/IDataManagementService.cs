using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.DataManagement;

public interface IDataManagementService
{
    /// <summary>
    /// UC13 - Obtenir un résumé de toutes les données
    /// </summary>
    Task<DataSummary> GetDataSummaryAsync(int clientId);
    
    /// <summary>
    /// UC14 - Supprimer toutes les entrées d'usage
    /// </summary>
    Task DeleteAllUsageDataAsync(int clientId);
    
    /// <summary>
    /// UC14 - Supprimer toutes les sessions de pause
    /// </summary>
    Task DeleteAllBreakSessionsAsync(int clientId);
    
    /// <summary>
    /// UC14 - Supprimer tous les blocages d'applications
    /// </summary>
    Task DeleteAllAppBlocksAsync(int clientId);
    
    /// <summary>
    /// UC14 - Supprimer toutes les alertes
    /// </summary>
    Task DeleteAllAlertsAsync(int clientId);
    
    /// <summary>
    /// UC14 - Supprimer tous les seuils personnalisés
    /// </summary>
    Task DeleteAllThresholdsAsync(int clientId);
    
    /// <summary>
    /// UC14 - Supprimer tous les objectifs
    /// </summary>
    Task DeleteAllGoalsAsync(int clientId);
    
    /// <summary>
    /// UC14 - Réinitialisation complète de toutes les données
    /// </summary>
    Task ResetAllDataAsync(int clientId);
}