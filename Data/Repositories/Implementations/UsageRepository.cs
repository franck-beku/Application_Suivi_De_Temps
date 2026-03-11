using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Repositories.Implementations;

public class UsageRepository : IUsageRepository
{
    private readonly AppDbContext _context;

    public UsageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UsageEntry?> GetUsageEntryAsync(int clientId, int appId, DateOnly date)
    {
        return await _context.UsageEntries
            .Include(u => u.App)
                .ThenInclude(a => a!.Category)
            .FirstOrDefaultAsync(u => 
                u.ClientId == clientId && 
                u.AppId == appId && 
                u.Date == date);
    }

    public async Task<List<UsageEntry>> GetDailyUsageAsync(int clientId, DateOnly date)
    {
        return await _context.UsageEntries
            .Include(u => u.App)
                .ThenInclude(a => a!.Category)
            .Where(u => u.ClientId == clientId && u.Date == date)
            .OrderByDescending(u => u.Minutes)
            .ToListAsync();
    }

    public async Task<List<UsageEntry>> GetUsageRangeAsync(int clientId, DateOnly startDate, DateOnly endDate)
    {
        return await _context.UsageEntries
            .Include(u => u.App)
                .ThenInclude(a => a!.Category)
            .Where(u => 
                u.ClientId == clientId && 
                u.Date >= startDate && 
                u.Date <= endDate)
            .OrderBy(u => u.Date)
            .ThenBy(u => u.App!.Name)
            .ToListAsync();
    }

    public async Task<UsageEntry> AddOrUpdateUsageAsync(int clientId, int appId, DateOnly date, int minutesToAdd)
    {
        var existing = await GetUsageEntryAsync(clientId, appId, date);

        if (existing != null)
        {
            // Mettre à jour l'entrée existante
            existing.Minutes += minutesToAdd;
            return await UpdateUsageEntryAsync(existing);
        }

        // Créer une nouvelle entrée
        var newEntry = new UsageEntry
        {
            ClientId = clientId,
            AppId = appId,
            Date = date,
            Minutes = minutesToAdd
        };

        return await CreateUsageEntryAsync(newEntry);
    }

    public async Task<UsageEntry> CreateUsageEntryAsync(UsageEntry entry)
    {
        _context.UsageEntries.Add(entry);
        await _context.SaveChangesAsync();
        
        // Recharger avec les relations
        return await GetUsageEntryAsync(entry.ClientId, entry.AppId, entry.Date) ?? entry;
    }

    public async Task<UsageEntry> UpdateUsageEntryAsync(UsageEntry entry)
    {
        _context.UsageEntries.Update(entry);
        await _context.SaveChangesAsync();
        return entry;
    }

    public async Task DeleteAllUsageAsync(int clientId)
    {
        var entries = await _context.UsageEntries
            .Where(u => u.ClientId == clientId)
            .ToListAsync();

        _context.UsageEntries.RemoveRange(entries);
        await _context.SaveChangesAsync();
    }
}