using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Data.Repositories.Interfaces;

public interface IThresholdRepository
{
    // AppThreshold
    Task<List<AppThreshold>> GetAppThresholdsByClientAsync(int clientId);
    Task<AppThreshold?> GetAppThresholdAsync(int clientId, int appId, ThresholdType type);
    Task<AppThreshold> CreateAppThresholdAsync(AppThreshold threshold);
    Task<AppThreshold> UpdateAppThresholdAsync(AppThreshold threshold);
    Task DeleteAppThresholdAsync(int thresholdId);
    
    // CategoryThreshold
    Task<List<CategoryThreshold>> GetCategoryThresholdsByClientAsync(int clientId);
    Task<CategoryThreshold?> GetCategoryThresholdAsync(int clientId, int categoryId, ThresholdType type);
    Task<CategoryThreshold> CreateCategoryThresholdAsync(CategoryThreshold threshold);
    Task<CategoryThreshold> UpdateCategoryThresholdAsync(CategoryThreshold threshold);
    Task DeleteCategoryThresholdAsync(int thresholdId);
}