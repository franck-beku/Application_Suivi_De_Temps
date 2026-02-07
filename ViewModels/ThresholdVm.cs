using Application_Suivi_De_Temps.Models;
using System.ComponentModel.DataAnnotations;

namespace Application_Suivi_De_Temps.ViewModels;

public class ThresholdVm
{
    public List<CategoryThresholdItem> CategoryThresholds { get; set; } = new();
    public List<AppThresholdItem> AppThresholds { get; set; } = new();
    
    // Pour l'ajout de nouveaux seuils
    public List<Category> AvailableCategories { get; set; } = new();
    public List<App> AvailableApps { get; set; } = new();
}

public class CategoryThresholdItem
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Le seuil est requis")]
    [Range(5, 720, ErrorMessage = "Le seuil doit être entre 5 minutes et 12 heures")]
    public int ThresholdMinutes { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Helper pour affichage
    public string ThresholdDisplay
    {
        get
        {
            var hours = ThresholdMinutes / 60;
            var minutes = ThresholdMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
}

public class AppThresholdItem
{
    public int AppId { get; set; }
    public string AppName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Le seuil est requis")]
    [Range(5, 720, ErrorMessage = "Le seuil doit être entre 5 minutes et 12 heures")]
    public int ThresholdMinutes { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Helper pour affichage
    public string ThresholdDisplay
    {
        get
        {
            var hours = ThresholdMinutes / 60;
            var minutes = ThresholdMinutes % 60;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }
    }
}