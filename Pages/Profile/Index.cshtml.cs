using Application_Suivi_De_Temps.Models;
using Application_Suivi_De_Temps.Services.Profile;
using Application_Suivi_De_Temps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Suivi_De_Temps.Pages.Profile;

public class IndexModel : PageModel
{
    private readonly IProfileService _profileService;
    private const int DefaultClientId = 1; // Pour simplifier, on utilise toujours le client 1

    public IndexModel(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [BindProperty]
    public ProfileVm ProfileData { get; set; } = new();

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var client = await _profileService.GetClientAsync(DefaultClientId);
        if (client == null)
        {
            ErrorMessage = "Client introuvable.";
            return Page();
        }

        var preferences = await _profileService.GetPreferencesAsync(DefaultClientId);

        // Mapper les données vers le ViewModel
        ProfileData = new ProfileVm
        {
            DisplayName = client.DisplayName,
            DefaultDailyGoalMinutes = preferences?.DefaultDailyGoalMinutes ?? 180,
            EnableNotifications = preferences?.EnableNotifications ?? true,
            EnableBreakReminders = preferences?.EnableBreakReminders ?? true,
            EnableWeeklyReport = preferences?.EnableWeeklyReport ?? true,
            DisconnectStartTime = preferences?.DisconnectStartTime,
            DisconnectEndTime = preferences?.DisconnectEndTime
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Veuillez corriger les erreurs dans le formulaire.";
            return Page();
        }

        try
        {
            // Mettre à jour le nom d'affichage
            await _profileService.UpdateClientNameAsync(DefaultClientId, ProfileData.DisplayName);

            // Mettre à jour les préférences
            var preferences = new UserPreferences
            {
                ClientId = DefaultClientId,
                DefaultDailyGoalMinutes = ProfileData.DefaultDailyGoalMinutes,
                EnableNotifications = ProfileData.EnableNotifications,
                EnableBreakReminders = ProfileData.EnableBreakReminders,
                EnableWeeklyReport = ProfileData.EnableWeeklyReport,
                DisconnectStartTime = ProfileData.DisconnectStartTime,
                DisconnectEndTime = ProfileData.DisconnectEndTime
            };

            await _profileService.UpdatePreferencesAsync(DefaultClientId, preferences);

            SuccessMessage = "Profil mis à jour avec succès !";
            TempData["SuccessMessage"] = SuccessMessage;
            
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur lors de la mise à jour : {ex.Message}";
            return Page();
        }
    }
}