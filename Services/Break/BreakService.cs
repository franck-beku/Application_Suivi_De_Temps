using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Services.Breaks;

public class BreakService : IBreakService
{
    private readonly IBreakRepository _breakRepo;

    public BreakService(IBreakRepository breakRepo)
    {
        _breakRepo = breakRepo;
    }

    public async Task<List<BreakActivity>> GetAllActivitiesAsync()
    {
        return await _breakRepo.GetAllActivitiesAsync();
    }

    public async Task<List<BreakActivity>> GetActivitiesByCategoryAsync(BreakCategory category)
    {
        return await _breakRepo.GetActivitiesByCategoryAsync(category);
    }

    public async Task<BreakSession> RecordBreakSessionAsync(int clientId, int activityId, int durationMinutes, string? notes = null)
    {
        var session = new BreakSession
        {
            ClientId = clientId,
            BreakActivityId = activityId,
            DurationMinutes = durationMinutes,
            Notes = notes
        };

        return await _breakRepo.CreateSessionAsync(session);
    }

    public async Task<List<BreakSession>> GetTodaySessionsAsync(int clientId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return await _breakRepo.GetSessionsAsync(clientId, today);
    }

    public async Task<BreakStats> GetBreakStatsAsync(int clientId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var sevenDaysAgo = today.AddDays(-6);

        // Total aujourd'hui
        var todayTotal = await _breakRepo.GetTotalBreakMinutesAsync(clientId, today);

        // Total cette semaine
        var weeklyTotal = await _breakRepo.GetTotalBreakMinutesByPeriodAsync(clientId, sevenDaysAgo, today);

        // Toutes les sessions de la semaine
        var weeklySessions = await _breakRepo.GetSessionsRangeAsync(clientId, sevenDaysAgo, today);
        var totalSessions = weeklySessions.Count;

        // Activité favorite (la plus utilisée)
        var favoriteActivity = weeklySessions
            .GroupBy(s => s.BreakActivity?.Name)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key ?? "Aucune";

        return new BreakStats
        {
            TodayTotalMinutes = todayTotal,
            WeeklyTotalMinutes = weeklyTotal,
            TotalSessions = totalSessions,
            FavoriteActivity = favoriteActivity
        };
    }

    public async Task DeleteAllSessionsAsync(int clientId)
    {
        await _breakRepo.DeleteAllSessionsAsync(clientId);
    }
}