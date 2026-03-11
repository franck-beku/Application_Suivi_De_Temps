using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Data.Repositories.Interfaces;

public interface IBadgeRepository
{
    Task<List<Badge>> GetAllBadgesAsync();
    Task<Badge?> GetBadgeByIdAsync(int id);
    Task<List<UserBadge>> GetUserBadgesAsync(int clientId);
    Task<UserBadge?> GetUserBadgeAsync(int clientId, int badgeId);
    Task<UserBadge> UnlockBadgeAsync(UserBadge userBadge);
    Task<int> GetUnlockedBadgesCountAsync(int clientId);
}