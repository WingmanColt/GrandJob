using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ISenderService _senderService;
        private readonly IBaseService _baseService;
        public ForgotPasswordModel(UserManager<User> userManager,
            ISenderService senderService, 
            IBaseService baseService)
        {
            _userManager = userManager;
            _senderService = senderService;
            _baseService = baseService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "Този емайл не съществува или операцията не може да бъде изпълнена! Моля уведомете администратор.", 10000);
                    return Page();
                }


                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                await _senderService.SendEmailAsync(Input.Email,
                    "Нулирай паролата си",
                    $"Можете да зададете нова парола, като кликнете на този <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>линк</a>.");
                _baseService.ToastNotify(ToastMessageState.Info, "", "Изпратихме ви емайл с линк към посочената от вас поща.", 3000);

                return RedirectToAction("Index", "Home");
            }

            return Page();
        }
    }
}
