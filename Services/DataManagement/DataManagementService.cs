using Application_Suivi_De_Temps.Data;
using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Services.DataManagement;

public class DataManagementService : IDataManagementService
{
    private readonly AppDbContext _context;

    public DataManagementService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DataSummary> GetDataSummaryAsync(int clientId)
    {
        var summary = new DataSummary();

        // Usage
        var usageEntries = await _context.UsageEntries
            .Where(u => u.ClientId == clientId)
            .ToListAsync();
        
        summary.TotalUsageEntries = usageEntries.Count;
        summary.TotalUsageMinutes = usageEntries.Sum(u => u.Minutes);
        summary.FirstUsageDate = usageEntries.Any() 
            ? usageEntries.Min(u => u.Date.ToDateTime(TimeOnly.MinValue)) 
            : null;
        summary.LastUsageDate = usageEntries.Any() 
            ? usageEntries.Max(u => u.Date.ToDateTime(TimeOnly.MinValue)) 
            : null;

        // Pauses
        var breakSessions = await _context.BreakSessions
            .Where(b => b.ClientId == clientId)
            .ToListAsync();
        
        summary.TotalBreakSessions = breakSessions.Count;
        summary.TotalBreakMinutes = breakSessions.Sum(b => b.DurationMinutes);

        // Seuils
        summary.TotalAppThresholds = await _context.AppThresholds
            .CountAsync(t => t.ClientId == clientId);
        summary.TotalCategoryThresholds = await _context.CategoryThresholds
            .CountAsync(t => t.ClientId == clientId);

        // Blocages
        var blocks = await _context.AppBlocks
            .Where(b => b.ClientId == clientId)
            .ToListAsync();
        
        summary.TotalAppBlocks = blocks.Count;
        summary.ActiveAppBlocks = blocks.Count(b => b.BlockedUntil > DateTime.UtcNow);

        // Alertes
        var alerts = await _context.AlertLogs
            .Where(a => a.ClientId == clientId)
            .ToListAsync();
        
        summary.TotalAlerts = alerts.Count;
        summary.UnreadAlerts = alerts.Count(a => !a.IsRead);

        // Objectifs
        var goals = await _context.Goals
            .Where(g => g.ClientId == clientId)
            .ToListAsync();
        
        summary.TotalGoals = goals.Count;
        summary.CompletedGoals = goals.Count(g => g.IsCompleted);

        // Badges
        summary.UnlockedBadges = await _context.UserBadges
            .CountAsync(ub => ub.ClientId == clientId);
        summary.TotalBadges = await _context.Badges.CountAsync();

        return summary;
    }

    public async Task DeleteAllUsageDataAsync(int clientId)
    {
        var entries = await _context.UsageEntries
            .Where(u => u.ClientId == clientId)
            .ToListAsync();

        _context.UsageEntries.RemoveRange(entries);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllBreakSessionsAsync(int clientId)
    {
        var sessions = await _context.BreakSessions
            .Where(b => b.ClientId == clientId)
            .ToListAsync();

        _context.BreakSessions.RemoveRange(sessions);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllAppBlocksAsync(int clientId)
    {
        var blocks = await _context.AppBlocks
            .Where(b => b.ClientId == clientId)
            .ToListAsync();

        _context.AppBlocks.RemoveRange(blocks);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllAlertsAsync(int clientId)
    {
        var alerts = await _context.AlertLogs
            .Where(a => a.ClientId == clientId)
            .ToListAsync();

        _context.AlertLogs.RemoveRange(alerts);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllThresholdsAsync(int clientId)
    {
        var appThresholds = await _context.AppThresholds
            .Where(t => t.ClientId == clientId)
            .ToListAsync();

        var categoryThresholds = await _context.CategoryThresholds
            .Where(t => t.ClientId == clientId)
            .ToListAsync();

        _context.AppThresholds.RemoveRange(appThresholds);
        _context.CategoryThresholds.RemoveRange(categoryThresholds);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllGoalsAsync(int clientId)
    {
        var goals = await _context.Goals
            .Where(g => g.ClientId == clientId)
            .ToListAsync();

        _context.Goals.RemoveRange(goals);
        await _context.SaveChangesAsync();
    }

    public async Task ResetAllDataAsync(int clientId)
    {
        // Supprimer dans l'ordre pour éviter les problèmes de clés étrangères
        await DeleteAllAlertsAsync(clientId);
        await DeleteAllAppBlocksAsync(clientId);
        await DeleteAllBreakSessionsAsync(clientId);
        await DeleteAllUsageDataAsync(clientId);
        await DeleteAllThresholdsAsync(clientId);
        await DeleteAllGoalsAsync(clientId);
        
        // Supprimer les badges débloqués
        var userBadges = await _context.UserBadges
            .Where(ub => ub.ClientId == clientId)
            .ToListAsync();
        
        _context.UserBadges.RemoveRange(userBadges);
        await _context.SaveChangesAsync();
    }
}