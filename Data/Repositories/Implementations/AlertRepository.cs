using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Repositories.Implementations;

public class AlertRepository : IAlertRepository
{
    private readonly AppDbContext _context;

    public AlertRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AlertLog> CreateAlertAsync(AlertLog alert)
    {
        alert.CreatedAt = DateTime.UtcNow;
        _context.AlertLogs.Add(alert);
        await _context.SaveChangesAsync();
        return alert;
    }

    public async Task<List<AlertLog>> GetRecentAlertsAsync(int clientId, int count = 10)
    {
        return await _context.AlertLogs
            .Include(a => a.App)
            .Include(a => a.Category)
            .Where(a => a.ClientId == clientId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<AlertLog>> GetUnreadAlertsAsync(int clientId)
    {
        return await _context.AlertLogs
            .Include(a => a.App)
            .Include(a => a.Category)
            .Where(a => a.ClientId == clientId && !a.IsRead)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<AlertLog>> GetAlertsAsync(int clientId, DateOnly? date = null)
    {
        var query = _context.AlertLogs
            .Include(a => a.App)
            .Include(a => a.Category)
            .Where(a => a.ClientId == clientId);

        if (date.HasValue)
        {
            var startDate = date.Value.ToDateTime(TimeOnly.MinValue);
            var endDate = date.Value.ToDateTime(TimeOnly.MaxValue);
            query = query.Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate);
        }

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(int alertId)
    {
        var alert = await _context.AlertLogs.FindAsync(alertId);
        if (alert != null)
        {
            alert.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(int clientId)
    {
        var alerts = await _context.AlertLogs
            .Where(a => a.ClientId == clientId && !a.IsRead)
            .ToListAsync();

        foreach (var alert in alerts)
        {
            alert.IsRead = true;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAlertAsync(int alertId)
    {
        var alert = await _context.AlertLogs.FindAsync(alertId);
        if (alert != null)
        {
            _context.AlertLogs.Remove(alert);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllAlertsAsync(int clientId)
    {
        var alerts = await _context.AlertLogs
            .Where(a => a.ClientId == clientId)
            .ToListAsync();

        _context.AlertLogs.RemoveRange(alerts);
        await _context.SaveChangesAsync();
    }
}