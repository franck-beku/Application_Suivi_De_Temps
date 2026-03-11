using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.Badges;

public class BadgeService : IBadgeService
{
    private readonly IBadgeRepository _badgeRepo;

    public BadgeService(IBadgeRepository badgeRepo)
    {
        _badgeRepo = badgeRepo;
    }

    public async Task<List<Badge>> GetAllBadgesAsync()
    {
        return await _badgeRepo.GetAllBadgesAsync();
    }

    public async Task<List<UserBadge>> GetUnlockedBadgesAsync(int clientId)
    {
        return await _badgeRepo.GetUserBadgesAsync(clientId);
    }

    public async Task<List<Badge>> GetLockedBadgesAsync(int clientId)
    {
        var allBadges = await _badgeRepo.GetAllBadgesAsync();
        var userBadges = await _badgeRepo.GetUserBadgesAsync(clientId);
        var unlockedBadgeIds = userBadges.Select(ub => ub.BadgeId).ToList();

        return allBadges.Where(b => !unlockedBadgeIds.Contains(b.Id)).ToList();
    }

    public async Task<UserBadge?> UnlockBadgeAsync(int clientId, int badgeId)
    {
        // Vérifier si déjà débloqué
        var existing = await _badgeRepo.GetUserBadgeAsync(clientId, badgeId);
        if (existing != null)
            return null;

        var userBadge = new UserBadge
        {
            ClientId = clientId,
            BadgeId = badgeId
        };

        return await _badgeRepo.UnlockBadgeAsync(userBadge);
    }

    public async Task<bool> IsBadgeUnlockedAsync(int clientId, int badgeId)
    {
        var userBadge = await _badgeRepo.GetUserBadgeAsync(clientId, badgeId);
        return userBadge != null;
    }

    public async Task<(int unlocked, int total, int percentage)> GetBadgeStatsAsync(int clientId)
    {
        var unlockedCount = await _badgeRepo.GetUnlockedBadgesCountAsync(clientId);
        var allBadges = await _badgeRepo.GetAllBadgesAsync();
        var totalCount = allBadges.Count;

        var percentage = totalCount > 0 ? (int)((unlockedCount / (double)totalCount) * 100) : 0;

        return (unlockedCount, totalCount, percentage);
    }
}