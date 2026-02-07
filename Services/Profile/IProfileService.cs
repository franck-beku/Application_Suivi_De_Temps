using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.Profile;

public interface IProfileService
{
    Task<Client?> GetClientAsync(int clientId);
    Task<Client> UpdateClientNameAsync(int clientId, string displayName);
    Task<UserPreferences?> GetPreferencesAsync(int clientId);
    Task<UserPreferences> UpdatePreferencesAsync(int clientId, UserPreferences preferences);
}