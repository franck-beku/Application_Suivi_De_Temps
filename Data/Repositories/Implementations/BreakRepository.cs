using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Repositories.Implementations;

public class BreakRepository : IBreakRepository
{
    private readonly AppDbContext _context;

    public BreakRepository(AppDbContext context)
    {
        _context = context;
    }

    // BreakActivities
    public async Task<List<BreakActivity>> GetAllActivitiesAsync()
    {
        return await _context.BreakActivities
            .Where(a => a.IsActive)
            .OrderBy(a => a.Category)
            .ThenBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<List<BreakActivity>> GetActivitiesByCategoryAsync(BreakCategory category)
    {
        return await _context.BreakActivities
            .Where(a => a.IsActive && a.Category == category)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<BreakActivity?> GetActivityByIdAsync(int id)
    {
        return await _context.BreakActivities.FindAsync(id);
    }

    // BreakSessions
    public async Task<BreakSession> CreateSessionAsync(BreakSession session)
    {
        session.StartedAt = DateTime.UtcNow;
        session.CompletedAt = session.StartedAt.AddMinutes(session.DurationMinutes);
        
        _context.BreakSessions.Add(session);
        await _context.SaveChangesAsync();
        
        // Recharger avec les relations
        return await _context.BreakSessions
            .Include(s => s.BreakActivity)
            .FirstOrDefaultAsync(s => s.Id == session.Id) ?? session;
    }

    public async Task<List<BreakSession>> GetSessionsAsync(int clientId, DateOnly? date = null)
    {
        var query = _context.BreakSessions
            .Include(s => s.BreakActivity)
            .Where(s => s.ClientId == clientId);

        if (date.HasValue)
        {
            var startDate = date.Value.ToDateTime(TimeOnly.MinValue);
            var endDate = date.Value.ToDateTime(TimeOnly.MaxValue);
            query = query.Where(s => s.StartedAt >= startDate && s.StartedAt <= endDate);
        }

        return await query
            .OrderByDescending(s => s.StartedAt)
            .ToListAsync();
    }

    public async Task<List<BreakSession>> GetSessionsRangeAsync(int clientId, DateOnly startDate, DateOnly endDate)
    {
        var start = startDate.ToDateTime(TimeOnly.MinValue);
        var end = endDate.ToDateTime(TimeOnly.MaxValue);

        return await _context.BreakSessions
            .Include(s => s.BreakActivity)
            .Where(s => s.ClientId == clientId && s.StartedAt >= start && s.StartedAt <= end)
            .OrderByDescending(s => s.StartedAt)
            .ToListAsync();
    }

    public async Task<int> GetTotalBreakMinutesAsync(int clientId, DateOnly date)
    {
        var startDate = date.ToDateTime(TimeOnly.MinValue);
        var endDate = date.ToDateTime(TimeOnly.MaxValue);

        return await _context.BreakSessions
            .Where(s => s.ClientId == clientId && s.StartedAt >= startDate && s.StartedAt <= endDate)
            .SumAsync(s => s.DurationMinutes);
    }

    public async Task<int> GetTotalBreakMinutesByPeriodAsync(int clientId, DateOnly startDate, DateOnly endDate)
    {
        var start = startDate.ToDateTime(TimeOnly.MinValue);
        var end = endDate.ToDateTime(TimeOnly.MaxValue);

        return await _context.BreakSessions
            .Where(s => s.ClientId == clientId && s.StartedAt >= start && s.StartedAt <= end)
            .SumAsync(s => s.DurationMinutes);
    }

    public async Task DeleteAllSessionsAsync(int clientId)
    {
        var sessions = await _context.BreakSessions
            .Where(s => s.ClientId == clientId)
            .ToListAsync();

        _context.BreakSessions.RemoveRange(sessions);
        await _context.SaveChangesAsync();
    }
}