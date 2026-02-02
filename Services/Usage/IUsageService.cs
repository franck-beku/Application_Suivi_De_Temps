namespace Application_Suivi_De_Temps.Services.Usage;

public interface IUsageService
{
    Task<int> GetTotalMinutesForDateAsync(int clientId, DateOnly date);
}
