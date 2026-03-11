using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Data.Repositories.Interfaces;

public interface IBreakRepository
{
    // BreakActivities
    Task<List<BreakActivity>> GetAllActivitiesAsync();
    Task<List<BreakActivity>> GetActivitiesByCategoryAsync(BreakCategory category);
    Task<BreakActivity?> GetActivityByIdAsync(int id);
    
    // BreakSessions
    Task<BreakSession> CreateSessionAsync(BreakSession session);
    Task<List<BreakSession>> GetSessionsAsync(int clientId, DateOnly? date = null);
    Task<List<BreakSession>> GetSessionsRangeAsync(int clientId, DateOnly startDate, DateOnly endDate);
    Task<int> GetTotalBreakMinutesAsync(int clientId, DateOnly date);
    Task<int> GetTotalBreakMinutesByPeriodAsync(int clientId, DateOnly startDate, DateOnly endDate);
    Task DeleteAllSessionsAsync(int clientId);
}