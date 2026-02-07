using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Data.Repositories.Interfaces;

public interface IUserPreferencesRepository
{
    Task<UserPreferences?> GetByClientIdAsync(int clientId);
    Task<UserPreferences> UpdateAsync(UserPreferences preferences);
    Task<UserPreferences> CreateAsync(UserPreferences preferences);
}