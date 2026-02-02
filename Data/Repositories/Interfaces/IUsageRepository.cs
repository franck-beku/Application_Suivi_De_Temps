using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Data.Repositories.Interfaces;

public interface IUsageRepository
{
    Task<List<UsageEntry>> GetByDateAsync(int clientId, DateOnly date);
    Task<List<UsageEntry>> GetBetweenAsync(int clientId, DateOnly from, DateOnly to);
}
