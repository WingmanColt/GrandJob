using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using HireMe.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly INotificationService _notifyService;
        private readonly IBaseService _baseService;
        private readonly ISenderService _senderService;
        private RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            INotificationService notifyService,
            IBaseService baseService,
            ILogger<RegisterModel> logger,
            ISenderService senderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _notifyService = notifyService;
            _baseService = baseService;
            _logger = logger;
            _senderService = senderService;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }


        public class InputModel
        {
            [Required]
            [EmailAddress]
            [EmailUserUnique]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (_signInManager.IsSignedIn(User))
            {
                return Redirect(returnUrl);
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect(returnUrl);
            }

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {

                var user = new User
                {
                    isExternal = false,
                    Email = Input.Email,
                    UserName = StringHelper.GetUntilOrEmpty(Input.Email, "@"),
                    PictureName = null
                };



                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var email = await _userManager.GetEmailAsync(user);

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _senderService.SendEmailAsync(email,
                    "Потвърди емайл адрес",
                    $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Потвърждаване</a>.");
                    var count = await _userManager.Users.CountAsync().ConfigureAwait(false);

                    if (count == 1)
                    {
                        await CreateRole();
                        await _userManager.AddToRoleAsync(user, "Admin");
                        await _userManager.RemoveFromRoleAsync(user, "User");

                        user.profileConfirmed = true;
                        user.EmailConfirmed = true;
                        user.Role = Roles.Admin;
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        user.Role = Roles.User;
                    }


                    await _notifyService.Create("Моля попълнете личните си данни.", "identity/account/manage/editprofile", DateTime.Now, NotifyType.Information, "fas fa-edit", user);

                    _baseService.ToastNotify(ToastMessageState.Alert, "Детайли", "Моля попълнете личните си данни.", 9000);
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "се регистрирахте. Благодарим ви за отделеното време !", 5000);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();

        }

        private async Task CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Moderator"));
            await _roleManager.CreateAsync(new IdentityRole("Employer"));
            await _roleManager.CreateAsync(new IdentityRole("Recruiter"));
            await _roleManager.CreateAsync(new IdentityRole("Contestant"));
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
    }
}
