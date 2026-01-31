using Microsoft.EntityFrameworkCore;
using Application_Suivi_De_Temps.Models;

namespace Application_Suivi_De_Temps.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; } 
  }
 }
