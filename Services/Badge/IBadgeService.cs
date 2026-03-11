using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.Badges;

public interface IBadgeService
{
    /// <summary>
    /// Obtenir tous les badges disponibles
    /// </summary>
    Task<List<Badge>> GetAllBadgesAsync();
    
    /// <summary>
    /// Obtenir les badges débloqués par un utilisateur
    /// </summary>
    Task<List<UserBadge>> GetUnlockedBadgesAsync(int clientId);
    
    /// <summary>
    /// Obtenir les badges encore verrouillés
    /// </summary>
    Task<List<Badge>> GetLockedBadgesAsync(int clientId);
    
    /// <summary>
    /// Débloquer un badge pour un utilisateur
    /// </summary>
    Task<UserBadge?> UnlockBadgeAsync(int clientId, int badgeId);
    
    /// <summary>
    /// Vérifier si un badge est débloqué
    /// </summary>
    Task<bool> IsBadgeUnlockedAsync(int clientId, int badgeId);
    
    /// <summary>
    /// Obtenir les statistiques de badges
    /// </summary>
    Task<(int unlocked, int total, int percentage)> GetBadgeStatsAsync(int clientId);
}