using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private readonly IBaseService _baseService;
        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, IBaseService baseService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _baseService = baseService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Запомни ме")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (_signInManager.IsSignedIn(User))
            {
                return Redirect(returnUrl);
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect(returnUrl);
            }

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                User user;
                if (Input.Email.Contains("@"))
                    user = await _userManager.FindByEmailAsync(Input.Email);
                else
                    user = await _userManager.FindByNameAsync(Input.Email);

                    var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _baseService.ToastNotify(ToastMessageState.Info, "Здравейте!", "Успешно влязохте в профила си.", 1000);
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _baseService.ToastNotifyLog(user, ToastMessageState.Error, "Проблем", "Профилът е заключен за 24 часа.", "login", 4000);
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    if(user.EmailConfirmed != true)
                    {
                     ModelState.AddModelError(string.Empty, "Моля потвърдете вашия емайл адрес за да влезете в системата ни !");
                    }
                    else
                    {
                     ModelState.AddModelError(string.Empty, "Възникна грешка, опитайте по-късно.");
                     _baseService.ToastNotifyLog(user, ToastMessageState.Error, "Проблем", "Възникна грешка, опитайте по-късно.", "login", 4000);
                    }

                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}


/*using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HireMe.Entities.Models;
using System.Threading;
using HireMe.Web.Controllers;
using HireMe.Services.Interfaces;
using HireMe.Entities.Enums;

namespace HireMe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IBaseService _baseService;
        public LoginModel(
            SignInManager<User> signInManager, 
            UserManager<User> userManager,
            IBaseService baseService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _baseService = baseService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("./Manage/Index");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("./Manage/Index");
            }

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                   
                    return RedirectToPage("./Manage/Index");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return RedirectToPage("./Manage/Index");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}



_baseService.ToastNotify(ToastMessageState.Success, "Здравейте!", "Успешно влязохте в профила си.", 4000);

*/

