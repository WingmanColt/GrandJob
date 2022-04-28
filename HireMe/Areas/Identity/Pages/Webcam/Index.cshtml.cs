namespace HireMe.Areas.Identity.Pages.Webcam
{
    using HireMe.Core.Extensions;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ITaskService _taskService;

        public IndexModel(UserManager<User> userManager,  ITaskService taskService)
        {
            _userManager = userManager;
            _taskService = taskService;
        }

        [BindProperty]
        public Tasks Tasks { get; set; }

        public async Task<IActionResult> OnGetAsync(string text)
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

            Tasks = await _taskService.GetByLinkAsync(text);

            if (Tasks is null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }
            if (!TimeExtension.IsBetween(DateTime.Now, Tasks.StartDate, Tasks.EndDate))
            {
                TempData["EndDate"] = Tasks.StartDate.ToString("yyyy-MM-ddTHH:mm:ss");
                Tasks.Behaviour = Entities.Enums.TasksBehaviour.Idle;
            }
            else
            {
                Tasks.Behaviour = Entities.Enums.TasksBehaviour.Running;
            }

            return Page();
        }

    }
}