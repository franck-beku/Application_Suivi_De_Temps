using Application_Suivi_De_Temps.Data.Repositories.Interfaces;

namespace Application_Suivi_De_Temps.Services.Usage;

public class UsageService : IUsageService
{
    private readonly IUsageRepository _repo;

    public UsageService(IUsageRepository repo) => _repo = repo;

    public async Task<int> GetTotalMinutesForDateAsync(int clientId, DateOnly date)
    {
        var entries = await _repo.GetByDateAsync(clientId, date);
        return entries.Sum(e => e.Minutes);
    }
}
