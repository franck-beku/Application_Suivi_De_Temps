using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Repositories.Implementations;

public class UserPreferencesRepository : IUserPreferencesRepository
{
    private readonly AppDbContext _context;

    public UserPreferencesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserPreferences?> GetByClientIdAsync(int clientId)
    {
        return await _context.UserPreferences
            .FirstOrDefaultAsync(p => p.ClientId == clientId);
    }

    public async Task<UserPreferences> UpdateAsync(UserPreferences preferences)
    {
        preferences.UpdatedAt = DateTime.UtcNow;
        _context.UserPreferences.Update(preferences);
        await _context.SaveChangesAsync();
        return preferences;
    }

    public async Task<UserPreferences> CreateAsync(UserPreferences preferences)
    {
        preferences.CreatedAt = DateTime.UtcNow;
        preferences.UpdatedAt = DateTime.UtcNow;
        _context.UserPreferences.Add(preferences);
        await _context.SaveChangesAsync();
        return preferences;
    }
}