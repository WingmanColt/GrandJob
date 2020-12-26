using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IBaseService _baseService;
        public LogoutModel(SignInManager<User> signInManager, IBaseService baseService)
        {
            _signInManager = signInManager;
            _baseService = baseService;
        }

        public async Task<IActionResult> OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
                _baseService.ToastNotify(ToastMessageState.Info, "Успешно", "излязохте от профила си.", 4000);
            }

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPost()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
                _baseService.ToastNotify(ToastMessageState.Info, "Успешно", "излязохте от профила си.", 1000);
            }

            return RedirectToPage("/Index");
        }
    }
}