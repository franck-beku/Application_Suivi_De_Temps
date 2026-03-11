using Application_Suivi_De_Temps.Data;
using Application_Suivi_De_Temps.Services.Usage;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Application_Suivi_De_Temps.Pages.Tracking;

public class IndexModel : PageModel
{
    private readonly IUsageService _usageService;
    private readonly AppDbContext _context;
    private const int DefaultClientId = 1;

    public IndexModel(IUsageService usageService, AppDbContext context)
    {
        _usageService = usageService;
        _context = context;
    }

    public TrackingVm Data { get; set; } = new();
    
    [BindProperty]
    public AddUsageInputModel Input { get; set; } = new();
    
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAddUsageAsync()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Veuillez corriger les erreurs dans le formulaire.";
            await LoadDataAsync();
            return Page();
        }

        try
        {
            var date = Input.Date ?? DateOnly.FromDateTime(DateTime.Today);
            await _usageService.RecordUsageAsync(DefaultClientId, Input.AppId, Input.Minutes, date);
            
            var app = await _context.Apps.FindAsync(Input.AppId);
            TempData["SuccessMessage"] = $"✅ {Input.Minutes} min ajoutées pour {app?.Name}";
            
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur : {ex.Message}";
            await LoadDataAsync();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostQuickAddAsync(int appId, int minutes)
    {
        try
        {
            await _usageService.RecordUsageAsync(DefaultClientId, appId, minutes);
            
            var app = await _context.Apps.FindAsync(appId);
            TempData["SuccessMessage"] = $"✅ {minutes} min ajoutées pour {app?.Name}";
            
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Erreur : {ex.Message}";
            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostResetTodayAsync()
    {
        try
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var entries = await _context.UsageEntries
                .Where(u => u.ClientId == DefaultClientId && u.Date == today)
                .ToListAsync();
            
            _context.UsageEntries.RemoveRange(entries);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Données du jour réinitialisées";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Erreur : {ex.Message}";
            return RedirectToPage();
        }
    }

    private async Task LoadDataAsync()
    {
        Data = new TrackingVm
        {
            TodaySummary = await _usageService.GetTodaySummaryAsync(DefaultClientId),
            AvailableApps = await _context.Apps
                .Include(a => a.Category)
                .OrderBy(a => a.Name)
                .ToListAsync(),
            QuickAddOptions = new List<QuickAddOption>
            {
                new() { Label = "5 min", Minutes = 5, BadgeClass = "bg-success" },
                new() { Label = "15 min", Minutes = 15, BadgeClass = "bg-info" },
                new() { Label = "30 min", Minutes = 30, BadgeClass = "bg-warning" },
                new() { Label = "1h", Minutes = 60, BadgeClass = "bg-danger" }
            }
        };
        
        // Initialiser Input avec des valeurs par défaut
        if (Data.AvailableApps.Any())
        {
            Input.AppId = Data.AvailableApps.First().Id;
        }
        Input.Minutes = 15;
        Input.Date = DateOnly.FromDateTime(DateTime.Today);
    }
}