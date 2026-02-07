using Application_Suivi_De_Temps.Data;
using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Services.Profile;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _context;
    private readonly IUserPreferencesRepository _preferencesRepo;

    public ProfileService(AppDbContext context, IUserPreferencesRepository preferencesRepo)
    {
        _context = context;
        _preferencesRepo = preferencesRepo;
    }

    public async Task<Client?> GetClientAsync(int clientId)
    {
        return await _context.Clients
            .Include(c => c.Preferences)
            .FirstOrDefaultAsync(c => c.Id == clientId);
    }

    public async Task<Client> UpdateClientNameAsync(int clientId, string displayName)
    {
        var client = await _context.Clients.FindAsync(clientId);
        if (client == null)
            throw new InvalidOperationException($"Client with ID {clientId} not found.");

        client.DisplayName = displayName;
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<UserPreferences?> GetPreferencesAsync(int clientId)
    {
        return await _preferencesRepo.GetByClientIdAsync(clientId);
    }

    public async Task<UserPreferences> UpdatePreferencesAsync(int clientId, UserPreferences preferences)
    {
        var existing = await _preferencesRepo.GetByClientIdAsync(clientId);
        
        if (existing == null)
        {
            preferences.ClientId = clientId;
            return await _preferencesRepo.CreateAsync(preferences);
        }

        existing.DefaultDailyGoalMinutes = preferences.DefaultDailyGoalMinutes;
        existing.EnableNotifications = preferences.EnableNotifications;
        existing.EnableBreakReminders = preferences.EnableBreakReminders;
        existing.EnableWeeklyReport = preferences.EnableWeeklyReport;
        existing.DisconnectStartTime = preferences.DisconnectStartTime;
        existing.DisconnectEndTime = preferences.DisconnectEndTime;

        return await _preferencesRepo.UpdateAsync(existing);
    }
}