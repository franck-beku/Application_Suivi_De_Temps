namespace Application_Suivi_De_Temps.Models;

public class UsageEntry
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    // Minutes d’utilisation de l’app sur la date
    public int Minutes { get; set; }

    // FK Client
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    // FK App
    public int AppId { get; set; }
    public App? App { get; set; }
}
