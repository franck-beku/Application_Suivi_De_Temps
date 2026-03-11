using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.Disconnect;

public class DisconnectService : IDisconnectService
{
    private readonly IUserPreferencesRepository _preferencesRepo;
    private readonly IBreakRepository _breakRepo;

    public DisconnectService(IUserPreferencesRepository preferencesRepo, IBreakRepository breakRepo)
    {
        _preferencesRepo = preferencesRepo;
        _breakRepo = breakRepo;
    }

    public async Task<bool> IsInDisconnectPeriodAsync(int clientId)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        return await IsTimeInDisconnectPeriodAsync(clientId, now);
    }

    public async Task<DisconnectStatus> GetDisconnectStatusAsync(int clientId)
    {
        var preferences = await _preferencesRepo.GetByClientIdAsync(clientId);

        if (preferences == null || !preferences.EnableDisconnectMode)
        {
            return new DisconnectStatus
            {
                IsEnabled = false,
                IsInDisconnectPeriod = false,
                Message = "Mode déconnexion désactivé"
            };
        }

        if (!preferences.DisconnectStartTime.HasValue || !preferences.DisconnectEndTime.HasValue)
        {
            return new DisconnectStatus
            {
                IsEnabled = true,
                IsInDisconnectPeriod = false,
                StartTime = preferences.DisconnectStartTime,
                EndTime = preferences.DisconnectEndTime,
                Message = "Horaires de déconnexion non configurés"
            };
        }

        var now = TimeOnly.FromDateTime(DateTime.Now);
        var isInPeriod = IsInPeriod(now, preferences.DisconnectStartTime.Value, preferences.DisconnectEndTime.Value);

        return new DisconnectStatus
        {
            IsEnabled = true,
            IsInDisconnectPeriod = isInPeriod,
            StartTime = preferences.DisconnectStartTime,
            EndTime = preferences.DisconnectEndTime,
            Message = isInPeriod 
                ? "Vous êtes dans votre plage de déconnexion 🌙" 
                : "Hors plage de déconnexion"
        };
    }

    public async Task<bool> IsTimeInDisconnectPeriodAsync(int clientId, TimeOnly time)
    {
        var preferences = await _preferencesRepo.GetByClientIdAsync(clientId);

        if (preferences == null || 
            !preferences.EnableDisconnectMode ||
            !preferences.DisconnectStartTime.HasValue || 
            !preferences.DisconnectEndTime.HasValue)
        {
            return false;
        }

        return IsInPeriod(time, preferences.DisconnectStartTime.Value, preferences.DisconnectEndTime.Value);
    }

    /// <summary>
    /// Vérifie si un moment est dans la période (gère les périodes qui passent minuit)
    /// </summary>
    private bool IsInPeriod(TimeOnly current, TimeOnly start, TimeOnly end)
    {
        // Cas normal : start < end (ex: 08:00 - 18:00)
        if (start < end)
        {
            return current >= start && current <= end;
        }
        
        // Cas spécial : la période passe minuit (ex: 22:00 - 07:00)
        // On est dans la période si :
        // - current >= start (après 22:00)
        // - OU current <= end (avant 07:00)
        return current >= start || current <= end;
    }

    /// <summary>
    /// UC8 - Suggérer une activité de pause aléatoire pour encourager la déconnexion
    /// </summary>
    public async Task<BreakActivity?> GetSuggestedBreakActivityAsync()
    {
        var allActivities = await _breakRepo.GetAllActivitiesAsync();
        
        if (!allActivities.Any())
            return null;
        
        // Sélectionner une activité aléatoire
        var random = new Random();
        var index = random.Next(allActivities.Count);
        return allActivities[index];
    }
}