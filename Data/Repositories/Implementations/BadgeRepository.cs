using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Repositories.Implementations;

public class BadgeRepository : IBadgeRepository
{
    private readonly AppDbContext _context;

    public BadgeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Badge>> GetAllBadgesAsync()
    {
        return await _context.Badges
            .OrderBy(b => b.RequiredValue)
            .ToListAsync();
    }

    public async Task<Badge?> GetBadgeByIdAsync(int id)
    {
        return await _context.Badges.FindAsync(id);
    }

    public async Task<List<UserBadge>> GetUserBadgesAsync(int clientId)
    {
        return await _context.UserBadges
            .Include(ub => ub.Badge)
            .Where(ub => ub.ClientId == clientId)
            .OrderByDescending(ub => ub.UnlockedAt)
            .ToListAsync();
    }

    public async Task<UserBadge?> GetUserBadgeAsync(int clientId, int badgeId)
    {
        return await _context.UserBadges
            .Include(ub => ub.Badge)
            .FirstOrDefaultAsync(ub => ub.ClientId == clientId && ub.BadgeId == badgeId);
    }

    public async Task<UserBadge> UnlockBadgeAsync(UserBadge userBadge)
    {
        userBadge.UnlockedAt = DateTime.UtcNow;
        _context.UserBadges.Add(userBadge);
        await _context.SaveChangesAsync();
        
        // Recharger avec la relation Badge
        return await _context.UserBadges
            .Include(ub => ub.Badge)
            .FirstOrDefaultAsync(ub => ub.Id == userBadge.Id) ?? userBadge;
    }

    public async Task<int> GetUnlockedBadgesCountAsync(int clientId)
    {
        return await _context.UserBadges
            .CountAsync(ub => ub.ClientId == clientId);
    }
}