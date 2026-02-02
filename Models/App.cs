namespace Application_Suivi_De_Temps.Models;

public class App
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    // FK
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    // Navigation
    public ICollection<UsageEntry> UsageEntries { get; set; } = new List<UsageEntry>();
}
