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
            .IsUnique(); // Un seul seuil par client/app/type

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
            .IsUnique(); // Un seul seuil par client/catégorie/type
    }
}