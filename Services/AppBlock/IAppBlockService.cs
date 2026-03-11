using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.AppBlocks;

public interface IAppBlockService
{
    /// <summary>
    /// Bloquer une application pour une durée donnée
    /// </summary>
    Task<AppBlock> BlockAppAsync(int clientId, int appId, int durationMinutes, string? reason = null);
    
    /// <summary>
    /// Débloquer une application
    /// </summary>
    Task UnblockAppAsync(int clientId, int appId);
    
    /// <summary>
    /// Vérifier si une app est bloquée
    /// </summary>
    Task<bool> IsAppBlockedAsync(int clientId, int appId);
    
    /// <summary>
    /// Obtenir toutes les apps actuellement bloquées
    /// </summary>
    Task<List<AppBlock>> GetActiveBlocksAsync(int clientId);
    
    /// <summary>
    /// Nettoyer les blocages expirés
    /// </summary>
    Task CleanExpiredBlocksAsync(int clientId);
    
    /// <summary>
    /// Supprimer tous les blocages
    /// </summary>
    Task ClearAllBlocksAsync(int clientId);
}