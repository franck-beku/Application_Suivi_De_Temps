using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Application_Suivi_De_Temps.Models
{
 public class Client
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = "Adolescent";
    
    // Navigation
    public UserPreferences? Preferences { get; set; }
    public ICollection<UsageEntry> UsageEntries { get; set; } = new List<UsageEntry>();
    public ICollection<AppThreshold> AppThresholds { get; set; } = new List<AppThreshold>();
    public ICollection<CategoryThreshold> CategoryThresholds { get; set; } = new List<CategoryThreshold>();
}
}
