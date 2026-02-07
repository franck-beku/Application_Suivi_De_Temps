using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Models.Enums;

namespace Application_Suivi_De_Temps.Services.Threshold;

public interface IThresholdService
{
    // AppThreshold
    Task<List<AppThreshold>> GetAppThresholdsAsync(int clientId);
    Task<AppThreshold> SetAppThresholdAsync(int clientId, int appId, int thresholdMinutes, ThresholdType type = ThresholdType.Daily);
    Task RemoveAppThresholdAsync(int clientId, int appId, ThresholdType type = ThresholdType.Daily);
    
    // CategoryThreshold
    Task<List<CategoryThreshold>> GetCategoryThresholdsAsync(int clientId);
    Task<CategoryThreshold> SetCategoryThresholdAsync(int clientId, int categoryId, int thresholdMinutes, ThresholdType type = ThresholdType.Daily);
    Task RemoveCategoryThresholdAsync(int clientId, int categoryId, ThresholdType type = ThresholdType.Daily);
    
    // Helpers
    Task<List<App>> GetAvailableAppsAsync();
    Task<List<Category>> GetAvailableCategoriesAsync();
}