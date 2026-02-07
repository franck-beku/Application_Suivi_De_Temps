using Application_Suivi_De_Temps.Data;
using Application_Suivi_De_Temps.Data.Repositories.Interfaces;
using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Services.Threshold;

public class ThresholdService : IThresholdService
{
    private readonly IThresholdRepository _thresholdRepo;
    private readonly AppDbContext _context;

    public ThresholdService(IThresholdRepository thresholdRepo, AppDbContext context)
    {
        _thresholdRepo = thresholdRepo;
        _context = context;
    }

    // ===== AppThreshold =====
    
    public async Task<List<AppThreshold>> GetAppThresholdsAsync(int clientId)
    {
        return await _thresholdRepo.GetAppThresholdsByClientAsync(clientId);
    }

    public async Task<AppThreshold> SetAppThresholdAsync(int clientId, int appId, int thresholdMinutes, ThresholdType type = ThresholdType.Daily)
    {
        var existing = await _thresholdRepo.GetAppThresholdAsync(clientId, appId, type);

        if (existing != null)
        {
            // Mettre à jour
            existing.ThresholdMinutes = thresholdMinutes;
            existing.IsActive = true;
            return await _thresholdRepo.UpdateAppThresholdAsync(existing);
        }

        // Créer
        var threshold = new AppThreshold
        {
            ClientId = clientId,
            AppId = appId,
            ThresholdMinutes = thresholdMinutes,
            Type = type,
            IsActive = true
        };

        return await _thresholdRepo.CreateAppThresholdAsync(threshold);
    }

    public async Task RemoveAppThresholdAsync(int clientId, int appId, ThresholdType type = ThresholdType.Daily)
    {
        var threshold = await _thresholdRepo.GetAppThresholdAsync(clientId, appId, type);
        if (threshold != null)
        {
            await _thresholdRepo.DeleteAppThresholdAsync(threshold.Id);
        }
    }

    // ===== CategoryThreshold =====
    
    public async Task<List<CategoryThreshold>> GetCategoryThresholdsAsync(int clientId)
    {
        return await _thresholdRepo.GetCategoryThresholdsByClientAsync(clientId);
    }

    public async Task<CategoryThreshold> SetCategoryThresholdAsync(int clientId, int categoryId, int thresholdMinutes, ThresholdType type = ThresholdType.Daily)
    {
        var existing = await _thresholdRepo.GetCategoryThresholdAsync(clientId, categoryId, type);

        if (existing != null)
        {
            // Mettre à jour
            existing.ThresholdMinutes = thresholdMinutes;
            existing.IsActive = true;
            return await _thresholdRepo.UpdateCategoryThresholdAsync(existing);
        }

        // Créer
        var threshold = new CategoryThreshold
        {
            ClientId = clientId,
            CategoryId = categoryId,
            ThresholdMinutes = thresholdMinutes,
            Type = type,
            IsActive = true
        };

        return await _thresholdRepo.CreateCategoryThresholdAsync(threshold);
    }

    public async Task RemoveCategoryThresholdAsync(int clientId, int categoryId, ThresholdType type = ThresholdType.Daily)
    {
        var threshold = await _thresholdRepo.GetCategoryThresholdAsync(clientId, categoryId, type);
        if (threshold != null)
        {
            await _thresholdRepo.DeleteCategoryThresholdAsync(threshold.Id);
        }
    }

    // ===== Helpers =====
    
    public async Task<List<App>> GetAvailableAppsAsync()
    {
        return await _context.Apps
            .Include(a => a.Category)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<List<Category>> GetAvailableCategoriesAsync()
    {
        return await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}