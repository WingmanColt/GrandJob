using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IBaseService _baseService;

        public ConfirmEmailModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IBaseService baseService)
        {
            _userManager = userManager;
            _baseService = baseService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return RedirectToPage("errors/ExternalLoginError");
            }
            else
            {
                if (!user.profileConfirmed)
                {
                    user.profileConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    await _signInManager.RefreshSignInAsync(user);

                    _baseService.ToastNotify(ToastMessageState.Success, "Информация", "Успешно потвърдихте избрания от вас емайл адрес.", 8000);
                    return RedirectToPage("Manage/Index");
                }
            }

            return Page();
        }
    }
}
