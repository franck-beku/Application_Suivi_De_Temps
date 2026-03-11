using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.Disconnect;

public interface IDisconnectService
{
    /// <summary>
    /// Vérifie si on est actuellement dans la plage de déconnexion
    /// </summary>
    Task<bool> IsInDisconnectPeriodAsync(int clientId);
    
    /// <summary>
    /// Obtient le statut complet de la déconnexion
    /// </summary>
    Task<DisconnectStatus> GetDisconnectStatusAsync(int clientId);
    
    /// <summary>
    /// Vérifie si un moment donné est dans la plage de déconnexion
    /// </summary>
    Task<bool> IsTimeInDisconnectPeriodAsync(int clientId, TimeOnly time);
    
    /// <summary>
    /// Obtenir une suggestion d'activité de pause pour le mode déconnexion (UC8)
    /// </summary>
    Task<BreakActivity?> GetSuggestedBreakActivityAsync();
}