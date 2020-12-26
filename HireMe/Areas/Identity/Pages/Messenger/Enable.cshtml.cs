using HireMe.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Messenger
{
    [Authorize]
    public partial class EnableMessengerModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public EnableMessengerModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

   
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.MessagesEnable)
            {
                return RedirectToPage("/Messenger/Index", new { Area = "Identity" });
            }

            if (user.AccountType == 0)
            {
                return RedirectToPage("/Account/Manage/Pricing", new { Area = "Identity" });
            }


            return Page();

        }
       
    }
}
