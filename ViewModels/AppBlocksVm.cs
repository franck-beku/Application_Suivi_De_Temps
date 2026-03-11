using Application_Suivi_De_Temps.Models;
using System.ComponentModel.DataAnnotations;

namespace Application_Suivi_De_Temps.ViewModels;

public class AppBlocksVm
{
    public List<App> AvailableApps { get; set; } = new();
    public List<AppBlock> ActiveBlocks { get; set; } = new();
    public List<BlockDurationOption> DurationOptions { get; set; } = new();
}

public class BlockDurationOption
{
    public string Label { get; set; } = string.Empty;
    public int Minutes { get; set; }
    public string BadgeClass { get; set; } = "bg-secondary";
}

public class BlockAppInputModel
{
    [Required(ErrorMessage = "Sélectionnez une application")]
    public int AppId { get; set; }
    
    [Required(ErrorMessage = "La durée est requise")]
    [Range(1, 1440, ErrorMessage = "La durée doit être entre 1 minute et 24 heures")]
    public int DurationMinutes { get; set; } = 30;
    
    [MaxLength(500, ErrorMessage = "La raison ne peut pas dépasser 500 caractères")]
    public string? Reason { get; set; }
}