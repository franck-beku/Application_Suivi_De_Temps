using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Data.Repositories.Interfaces;

public interface IAlertRepository
{
    Task<AlertLog> CreateAlertAsync(AlertLog alert);
    Task<List<AlertLog>> GetRecentAlertsAsync(int clientId, int count = 10);
    Task<List<AlertLog>> GetUnreadAlertsAsync(int clientId);
    Task<List<AlertLog>> GetAlertsAsync(int clientId, DateOnly? date = null);
    Task MarkAsReadAsync(int alertId);
    Task MarkAllAsReadAsync(int clientId);
    Task DeleteAlertAsync(int alertId);
    Task DeleteAllAlertsAsync(int clientId);
}