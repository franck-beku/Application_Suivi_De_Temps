using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Application_Suivi_De_Temps.Models;

public class Client
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = "Adolescent";
    
    // Navigation
    public UserPreferences? Preferences { get; set; }
    public ICollection<UsageEntry> UsageEntries { get; set; } = new List<UsageEntry>();
    public ICollection<AppThreshold> AppThresholds { get; set; } = new List<AppThreshold>();
    public ICollection<CategoryThreshold> CategoryThresholds { get; set; } = new List<CategoryThreshold>();
    public ICollection<AlertLog> AlertLogs { get; set; } = new List<AlertLog>();
    public ICollection<BreakSession> BreakSessions { get; set; } = new List<BreakSession>();
    public ICollection<AppBlock> AppBlocks { get; set; } = new List<AppBlock>();
    public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
}