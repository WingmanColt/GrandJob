namespace HireMe.Areas.Identity.Pages.Checkout
{
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    public partial class CancelModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IContestantsService _contestantsService;
        private readonly IJobsService _jobsService;
        private readonly IBaseService _baseService;
        private readonly IPromotionService _promotionService;

        public CancelModel(
            UserManager<User> userManager,
            IJobsService jobsService,
            IContestantsService contestantsService,
            IBaseService baseService,
            IPromotionService promotionService)
        {
            _userManager = userManager;
            _baseService = baseService;
            _contestantsService = contestantsService;
            _jobsService = jobsService;
            _promotionService = promotionService;
        }




            public async Task<IActionResult> OnGetAsync(int package, int productId, int postType)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.AccountType == 0)
            {
                return RedirectToPage("/Account/Manage/Pricing", new { Area = "Identity" });
            }



            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            
            return RedirectToPage();
        }

    }
}