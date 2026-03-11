using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<UserPreferences> UserPreferences => Set<UserPreferences>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<App> Apps => Set<App>();
    public DbSet<UsageEntry> UsageEntries => Set<UsageEntry>();
    public DbSet<AppThreshold> AppThresholds => Set<AppThreshold>();
    public DbSet<CategoryThreshold> CategoryThresholds => Set<CategoryThreshold>();
    public DbSet<AlertLog> AlertLogs => Set<AlertLog>();
    public DbSet<BreakActivity> BreakActivities => Set<BreakActivity>();
    public DbSet<BreakSession> BreakSessions => Set<BreakSession>();
    public DbSet<AppBlock> AppBlocks => Set<AppBlock>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<Badge> Badges => Set<Badge>();
    public DbSet<UserBadge> UserBadges => Set<UserBadge>();
    public DbSet<Tip> Tips => Set<Tip>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Client
        modelBuilder.Entity<Client>()
            .Property(c => c.DisplayName)
            .HasMaxLength(100)
            .IsRequired();

        // UserPreferences - Relation 1:1 avec Client
        modelBuilder.Entity<UserPreferences>()
            .HasOne(p => p.Client)
            .WithOne(c => c.Preferences)
            .HasForeignKey<UserPreferences>(p => p.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserPreferences>()
            .Property(p => p.DefaultDailyGoalMinutes)
            .HasDefaultValue(180);

        modelBuilder.Entity<UserPreferences>()
            .Property(p => p.EnableNotifications)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserPreferences>()
            .Property(p => p.EnableBreakReminders)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserPreferences>()
            .Property(p => p.EnableWeeklyReport)
            .HasDefaultValue(true);

        modelBuilder.Entity<UserPreferences>()
            .Property(p => p.EnableDisconnectMode)
            .HasDefaultValue(false);

        // Category
        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        // App
        modelBuilder.Entity<App>()
            .Property(a => a.Name)
            .HasMaxLength(60)
            .IsRequired();

        modelBuilder.Entity<App>()
            .HasOne(a => a.Category)
            .WithMany(c => c.Apps)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // UsageEntry
        modelBuilder.Entity<UsageEntry>()
            .HasOne(u => u.Client)
            .WithMany(c => c.UsageEntries)
            .HasForeignKey(u => u.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UsageEntry>()
            .HasOne(u => u.App)
            .WithMany(a => a.UsageEntries)
            .HasForeignKey(u => u.AppId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UsageEntry>()
            .Property(u => u.Minutes)
            .HasDefaultValue(0);

        // AppThreshold
        modelBuilder.Entity<AppThreshold>()
            .HasOne(t => t.Client)
            .WithMany(c => c.AppThresholds)
            .HasForeignKey(t => t.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppThreshold>()
            .HasOne(t => t.App)
            .WithMany()
            .HasForeignKey(t => t.AppId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppThreshold>()
            .HasIndex(t => new { t.ClientId, t.AppId, t.Type })
            .IsUnique();

        // CategoryThreshold
        modelBuilder.Entity<CategoryThreshold>()
            .HasOne(t => t.Client)
            .WithMany(c => c.CategoryThresholds)
            .HasForeignKey(t => t.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CategoryThreshold>()
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CategoryThreshold>()
            .HasIndex(t => new { t.ClientId, t.CategoryId, t.Type })
            .IsUnique();

        // AlertLog
        modelBuilder.Entity<AlertLog>()
            .HasOne(a => a.Client)
            .WithMany(c => c.AlertLogs)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AlertLog>()
            .HasOne(a => a.App)
            .WithMany()
            .HasForeignKey(a => a.AppId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AlertLog>()
            .HasOne(a => a.Category)
            .WithMany()
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AlertLog>()
            .Property(a => a.Title)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<AlertLog>()
            .Property(a => a.Message)
            .HasMaxLength(500);

        modelBuilder.Entity<AlertLog>()
            .Property(a => a.IsRead)
            .HasDefaultValue(false);

        modelBuilder.Entity<AlertLog>()
            .HasIndex(a => new { a.ClientId, a.CreatedAt });

        // BreakActivity
        modelBuilder.Entity<BreakActivity>()
            .Property(b => b.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<BreakActivity>()
            .Property(b => b.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<BreakActivity>()
            .Property(b => b.IconClass)
            .HasMaxLength(50);

        modelBuilder.Entity<BreakActivity>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

        // BreakSession
        modelBuilder.Entity<BreakSession>()
            .HasOne(s => s.Client)
            .WithMany(c => c.BreakSessions)
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BreakSession>()
            .HasOne(s => s.BreakActivity)
            .WithMany(a => a.BreakSessions)
            .HasForeignKey(s => s.BreakActivityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BreakSession>()
            .Property(s => s.Notes)
            .HasMaxLength(500);

        modelBuilder.Entity<BreakSession>()
            .HasIndex(s => new { s.ClientId, s.StartedAt });

        // AppBlock
        modelBuilder.Entity<AppBlock>()
            .HasOne(b => b.Client)
            .WithMany(c => c.AppBlocks)
            .HasForeignKey(b => b.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppBlock>()
            .HasOne(b => b.App)
            .WithMany()
            .HasForeignKey(b => b.AppId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppBlock>()
            .Property(b => b.Reason)
            .HasMaxLength(500);

        modelBuilder.Entity<AppBlock>()
            .HasIndex(b => new { b.ClientId, b.AppId, b.BlockedUntil });

        // Goal (UC10)
        modelBuilder.Entity<Goal>()
            .HasOne(g => g.Client)
            .WithMany(c => c.Goals)
            .HasForeignKey(g => g.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Goal>()
            .Property(g => g.Title)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Goal>()
            .Property(g => g.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<Goal>()
            .HasIndex(g => new { g.ClientId, g.IsCompleted, g.EndDate });

        // Badge (UC11)
        modelBuilder.Entity<Badge>()
            .Property(b => b.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Badge>()
            .Property(b => b.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<Badge>()
            .Property(b => b.IconClass)
            .HasMaxLength(50);

        modelBuilder.Entity<Badge>()
            .Property(b => b.Color)
            .HasMaxLength(20);

        modelBuilder.Entity<Badge>()
            .Property(b => b.UnlockCriteria)
            .HasMaxLength(200);

        // UserBadge (UC11)
        modelBuilder.Entity<UserBadge>()
            .HasOne(ub => ub.Client)
            .WithMany(c => c.UserBadges)
            .HasForeignKey(ub => ub.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserBadge>()
            .HasOne(ub => ub.Badge)
            .WithMany(b => b.UserBadges)
            .HasForeignKey(ub => ub.BadgeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserBadge>()
            .HasIndex(ub => new { ub.ClientId, ub.BadgeId })
            .IsUnique();

        // Tip (UC12)
        modelBuilder.Entity<Tip>()
            .Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Tip>()
            .Property(t => t.Content)
            .HasMaxLength(1000)
            .IsRequired();

        modelBuilder.Entity<Tip>()
            .Property(t => t.IconClass)
            .HasMaxLength(50);

        modelBuilder.Entity<Tip>()
            .Property(t => t.IsActive)
            .HasDefaultValue(true);
    }
}