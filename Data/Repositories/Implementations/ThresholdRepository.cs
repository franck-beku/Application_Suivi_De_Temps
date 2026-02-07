using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data.Repositories.Implementations;

public class ThresholdRepository : IThresholdRepository
{
    private readonly AppDbContext _context;

    public ThresholdRepository(AppDbContext context)
    {
        _context = context;
    }

    // ===== AppThreshold =====
    
    public async Task<List<AppThreshold>> GetAppThresholdsByClientAsync(int clientId)
    {
        return await _context.AppThresholds
            .Include(t => t.App)
                .ThenInclude(a => a!.Category)
            .Where(t => t.ClientId == clientId && t.IsActive)
            .OrderBy(t => t.App!.Name)
            .ToListAsync();
    }

    public async Task<AppThreshold?> GetAppThresholdAsync(int clientId, int appId, ThresholdType type)
    {
        return await _context.AppThresholds
            .Include(t => t.App)
            .FirstOrDefaultAsync(t => 
                t.ClientId == clientId && 
                t.AppId == appId && 
                t.Type == type);
    }

    public async Task<AppThreshold> CreateAppThresholdAsync(AppThreshold threshold)
    {
        threshold.CreatedAt = DateTime.UtcNow;
        threshold.UpdatedAt = DateTime.UtcNow;
        _context.AppThresholds.Add(threshold);
        await _context.SaveChangesAsync();
        return threshold;
    }

    public async Task<AppThreshold> UpdateAppThresholdAsync(AppThreshold threshold)
    {
        threshold.UpdatedAt = DateTime.UtcNow;
        _context.AppThresholds.Update(threshold);
        await _context.SaveChangesAsync();
        return threshold;
    }

    public async Task DeleteAppThresholdAsync(int thresholdId)
    {
        var threshold = await _context.AppThresholds.FindAsync(thresholdId);
        if (threshold != null)
        {
            _context.AppThresholds.Remove(threshold);
            await _context.SaveChangesAsync();
        }
    }

    // ===== CategoryThreshold =====
    
    public async Task<List<CategoryThreshold>> GetCategoryThresholdsByClientAsync(int clientId)
    {
        return await _context.CategoryThresholds
            .Include(t => t.Category)
            .Where(t => t.ClientId == clientId && t.IsActive)
            .OrderBy(t => t.Category!.Name)
            .ToListAsync();
    }

    public async Task<CategoryThreshold?> GetCategoryThresholdAsync(int clientId, int categoryId, ThresholdType type)
    {
        return await _context.CategoryThresholds
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => 
                t.ClientId == clientId && 
                t.CategoryId == categoryId && 
                t.Type == type);
    }

    public async Task<CategoryThreshold> CreateCategoryThresholdAsync(CategoryThreshold threshold)
    {
        threshold.CreatedAt = DateTime.UtcNow;
        threshold.UpdatedAt = DateTime.UtcNow;
        _context.CategoryThresholds.Add(threshold);
        await _context.SaveChangesAsync();
        return threshold;
    }

    public async Task<CategoryThreshold> UpdateCategoryThresholdAsync(CategoryThreshold threshold)
    {
        threshold.UpdatedAt = DateTime.UtcNow;
        _context.CategoryThresholds.Update(threshold);
        await _context.SaveChangesAsync();
        return threshold;
    }

    public async Task DeleteCategoryThresholdAsync(int thresholdId)
    {
        var threshold = await _context.CategoryThresholds.FindAsync(thresholdId);
        if (threshold != null)
        {
            _context.CategoryThresholds.Remove(threshold);
            await _context.SaveChangesAsync();
        }
    }
}