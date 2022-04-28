using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class AdminModel : PageModel
    {
        private readonly INotificationService _notifyService;
        private readonly UserManager<User> _userManager;

        private readonly ICategoriesService _categoriesService;
        private readonly ILocationService _locationService;
        private readonly ILanguageService _langService;
        private readonly ISkillsService _skillsService;
        public AdminModel(UserManager<User> userManager, 
            INotificationService notifyService,
            ICategoriesService categoriesService,
            ILocationService locationService,
            ILanguageService langService,
            ISkillsService skillsService
            )
        {
            _userManager = userManager;
            _notifyService = notifyService;
            _categoriesService = categoriesService;
            _locationService = locationService;
            _skillsService = skillsService;
            _langService = langService;
        }


        public async Task<IActionResult> OnPostSyncAsync(string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.Role != Roles.Admin)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            // Categories seed
            var catategoriesResult = await _categoriesService.SeedCategories();

            if(catategoriesResult.Success)    
                await _notifyService.Create("Categories were added succesfuly.", $"categories/index", DateTime.Now, NotifyType.Success, "flaticon-database", user.Id, null).ConfigureAwait(false);
            else
                await _notifyService.Create($"{catategoriesResult.FailureMessage}", $"categories/index", DateTime.Now, NotifyType.Danger, "flaticon-database", user.Id, null).ConfigureAwait(false);


            // Locations seed
            var locationsResult = await _locationService.SeedLocation();

            if (locationsResult.Success)
                await _notifyService.Create("Locations were added succesfuly.", $"categories/index", DateTime.Now, NotifyType.Success, "flaticon-database", user.Id, null).ConfigureAwait(false);
            else
                await _notifyService.Create($"{locationsResult.FailureMessage}", $"categories/index", DateTime.Now, NotifyType.Danger, "flaticon-database", user.Id, null).ConfigureAwait(false);

            // Languages seed
            var languagesResult = await _langService.SeedLanguages();

            if (languagesResult.Success)
                await _notifyService.Create("Languages were added succesfuly.", $"categories/index", DateTime.Now, NotifyType.Success, "flaticon-database", user.Id, null).ConfigureAwait(false);
            else
                await _notifyService.Create($"{languagesResult.FailureMessage}", $"categories/index", DateTime.Now, NotifyType.Danger, "flaticon-database", user.Id, null).ConfigureAwait(false);


            // Skills seed
            var skillsResult = await _skillsService.SeedSkills();

            if (skillsResult.Success)
                await _notifyService.Create("Skills were added succesfuly.", $"categories/index", DateTime.Now, NotifyType.Success, "flaticon-database", user.Id, null).ConfigureAwait(false);
            else
                await _notifyService.Create($"{skillsResult.FailureMessage}", $"categories/index", DateTime.Now, NotifyType.Danger, "flaticon-database", user.Id, null).ConfigureAwait(false);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToPage("/Account/Manage/Index", new { Area = "Identity" });
        }
       
    }
}