﻿using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services;
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
        private readonly ILogService _logger;
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
            ILogService logger,
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

        [TempData]
        public string ErrorMessage { get; set; }

        public bool getEmployer { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [EmailUserUnique]
            [Display(Name = "Емайл")]
            public string Email { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = "{0}то трябва да е поне {2} и максимум {1} символи.", MinimumLength = 3)]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = "{0}та трябва да е поне {2} и максимум {1} символи.", MinimumLength = 3)]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0}та трябва да е поне {2} и максимум {1} символи.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърди паролата")]
            [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
            public string ConfirmPassword { get; set; }

            public bool isEmployer { get; set; }
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task<IActionResult> OnGetAsync(string employer = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect("./Manage/Index");
            }

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            if (employer?.Length > 0)
                getEmployer = true;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool e)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("./Manage/Index");
            }


            getEmployer = e;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    isExternal = false,
                    Email = Input.Email,
                    UserName = StringHelper.GetUntilOrEmpty(Input.Email, "@"),
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Role =  (Input.isEmployer || getEmployer) ? Roles.Employer : Roles.User,
                    PictureName = "200x200.jpg"
                };


                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var email = await _userManager.GetEmailAsync(user);

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                   var emailResult = await _senderService.SendEmailAsync(email, "Потвърди емайл адрес", callbackUrl).ConfigureAwait(false);
                    if (!emailResult.Success)
                        await _logger.Create(emailResult.FailureMessage, "RegisterPost", Entities.Enums.LogLevel.Danger, Input.Email);

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
                        if (!Input.isEmployer)
                        {
                            await _userManager.AddToRoleAsync(user, "User");
                            user.Role = Roles.User;
                        } 
                        else
                        {
                            await _userManager.AddToRoleAsync(user, "Employer");
                            user.Role = Roles.Employer;
                        }
                    }


                    await _notifyService.Create("Моля попълнете личните си данни.", "identity/account/manage/edit-profile", DateTime.Now, NotifyType.Information, null, user.Id, null).ConfigureAwait(false);

                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "се регистрирахте. Благодарим ви за отделеното време !", 5000);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, isEmployer = Input.isEmployer ? true : false });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToPage("./Manage/Index");
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
