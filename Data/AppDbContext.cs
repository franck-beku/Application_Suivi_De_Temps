using Microsoft.EntityFrameworkCore;
using Application_Suivi_De_Temps.Models;

/*namespace Application_Suivi_De_Temps.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; } 
  }
}*/

using Application_Suivi_De_Temps.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<App> Apps => Set<App>();
    public DbSet<UsageEntry> UsageEntries => Set<UsageEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
    }
}

