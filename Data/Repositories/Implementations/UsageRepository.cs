using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Repositories.Implementations;

public class UsageRepository : IUsageRepository
{
    private readonly AppDbContext _db;

    public UsageRepository(AppDbContext db) => _db = db;

    public Task<List<UsageEntry>> GetByDateAsync(int clientId, DateOnly date)
    {
        return _db.UsageEntries
            .Include(u => u.App)
            .ThenInclude(a => a!.Category)
            .Where(u => u.ClientId == clientId && u.Date == date)
            .ToListAsync();
    }

    public Task<List<UsageEntry>> GetBetweenAsync(int clientId, DateOnly from, DateOnly to)
    {
        return _db.UsageEntries
            .Include(u => u.App)
            .ThenInclude(a => a!.Category)
            .Where(u => u.ClientId == clientId && u.Date >= from && u.Date <= to)
            .ToListAsync();
    }
}
