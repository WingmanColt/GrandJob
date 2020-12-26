using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginErrorModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IBaseService _baseService;
        private readonly ISenderService _senderService;

        public ExternalLoginErrorModel(
            UserManager<User> userManager,
            IBaseService baseService,
            ISenderService senderService)
        {
            _userManager = userManager;
            _baseService = baseService;
            _senderService = senderService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            await _senderService.SendEmailAsync(email,
                "Потвърди емайл адрес",
                 $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> <img src='https://i.imgur.com/MOYSeFJ.jpg'> </a> <br>Изпратено с <3 от GrandJob.eu <br>София, Младост 4.");

            _baseService.ToastNotify(ToastMessageState.Info, "Информация", "Емайлът за потвърждение е изпратен. Моля проверете!", 4000);
            return RedirectToPage();
        }
    }
}
