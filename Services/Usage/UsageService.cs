using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Services.Usage;

public class UsageService : IUsageService
{
    private readonly IUsageRepository _usageRepo;

    public UsageService(IUsageRepository usageRepo)
    {
        _usageRepo = usageRepo;
    }

    public async Task<UsageEntry> RecordUsageAsync(int clientId, int appId, int minutes, DateOnly? date = null)
    {
        var targetDate = date ?? DateOnly.FromDateTime(DateTime.Today);
        return await _usageRepo.AddOrUpdateUsageAsync(clientId, appId, targetDate, minutes);
    }

    public async Task<UsageSummary> GetDailySummaryAsync(int clientId, DateOnly date)
    {
        var entries = await _usageRepo.GetDailyUsageAsync(clientId, date);
        
        var totalMinutes = entries.Sum(e => e.Minutes);
        
        var summary = new UsageSummary
        {
            Date = date,
            TotalMinutes = totalMinutes
        };

        // Calculer les usages par app
        summary.AppUsages = entries
            .Select(e => new AppUsage
            {
                AppId = e.AppId,
                AppName = e.App?.Name ?? "Unknown",
                CategoryName = e.App?.Category?.Name ?? "Unknown",
                Minutes = e.Minutes,
                Percentage = totalMinutes > 0 ? (int)((e.Minutes / (double)totalMinutes) * 100) : 0
            })
            .OrderByDescending(a => a.Minutes)
            .ToList();

        // Calculer les usages par catégorie
        var categoryGroups = entries
            .GroupBy(e => new { e.App!.CategoryId, e.App.Category!.Name })
            .Select(g => new CategoryUsage
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                Minutes = g.Sum(e => e.Minutes),
                AppCount = g.Select(e => e.AppId).Distinct().Count(),
                Percentage = totalMinutes > 0 ? (int)((g.Sum(e => e.Minutes) / (double)totalMinutes) * 100) : 0
            })
            .OrderByDescending(c => c.Minutes)
            .ToList();

        summary.CategoryUsages = categoryGroups;

        return summary;
    }

    public async Task<UsageSummary> GetTodaySummaryAsync(int clientId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return await GetDailySummaryAsync(clientId, today);
    }

    public async Task<int> GetTodayTotalMinutesAsync(int clientId)
    {
        var summary = await GetTodaySummaryAsync(clientId);
        return summary.TotalMinutes;
    }

    public async Task<int> GetPeriodTotalMinutesAsync(int clientId, DateOnly startDate, DateOnly endDate)
    {
        var entries = await _usageRepo.GetUsageRangeAsync(clientId, startDate, endDate);
        return entries.Sum(e => e.Minutes);
    }

    public async Task<List<UsageEntry>> GetUsageEntriesAsync(int clientId, DateOnly startDate, DateOnly endDate)
    {
        return await _usageRepo.GetUsageRangeAsync(clientId, startDate, endDate);
    }

    public async Task ResetAllUsageAsync(int clientId)
    {
        await _usageRepo.DeleteAllUsageAsync(clientId);
    }
}