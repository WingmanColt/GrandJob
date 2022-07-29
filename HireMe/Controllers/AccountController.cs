using HireMe.Controllers;
using HireMe.Entities.Enums;
using HireMe.Core.Helpers;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using HireMe.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HireMe.Web.Controllers
{
   // [Route("[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        private readonly IBaseService _baseService;
        private readonly IJobsService _jobsService;
        private readonly ICompanyService _companyService;
        private readonly IContestantsService _contestantService;
        private readonly IResumeService _resumeService;
        private readonly ISenderService _senderService;
        private readonly INotificationService _notifyService;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IBaseService baseService,
            IJobsService jobsService,
            ICompanyService companyService,
            IContestantsService contestantService,
            IResumeService resumeService,
            ISenderService senderService,
            INotificationService notifyService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

            _baseService = baseService;
            _jobsService = jobsService;
            _companyService = companyService;
            _contestantService = contestantService;
            _resumeService = resumeService;
            _senderService = senderService;
            _notifyService = notifyService;
        }


        // route is like that cuz we are using ajax url in script.js !
        [HttpPost]
        [Route("account/login")]
        public async Task<IActionResult> Login(AccountViewModel viewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // valid problems
            // viewModel.ConfirmPassword = viewModel.Password;

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                User user = await _userManager.FindByEmailAsync(viewModel.Email);

                if (user is null)
                {
                    viewModel.ErrorMessage = "Възникна грешка, не съществува такъв потребител.";

                    return NotFound(viewModel);
                }
                var result = await _signInManager.PasswordSignInAsync(user?.UserName, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _baseService.ToastNotify(ToastMessageState.Info, "Здравейте!", "Успешно влязохте в профила си.", 1000);
                    return Json(viewModel);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = viewModel.ReturnUrl, RememberMe = viewModel.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Проблем", "Профилът е заключен за 24 часа.", "login", 4000);
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    if (user.EmailConfirmed != true)
                    {
                        viewModel.ErrorMessage = "Моля потвърдете вашия емайл адрес за да влезете в системата ни !";
                    }
                    else
                    {
                        viewModel.ErrorMessage = "Възникна грешка, моля опитайте по-късно.";
                        await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Възникна грешка", "Грешна парола или емайл адрес.", "login", 10000);
                    }
                    return NotFound(viewModel);
                }
            }  

            viewModel.ErrorMessage = "Възникна грешка, грешна парола или емайл адрес.";
            return NotFound(viewModel);
        }


        [HttpPost]
        [Route("/Account/Register")]
        public async Task<IActionResult> Register(AccountViewModel viewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (ModelState.IsValid)
            {

                var user = new User
                {
                    isExternal = false,
                    Email = viewModel.Email,
                    UserName = StringHelper.GetUntilOrEmpty(viewModel.Email, "@"),
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    PictureName = "200x200.jpg"
                };



                var result = await _userManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var email = await _userManager.GetEmailAsync(user);

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _senderService.SendEmailAsync(email, "Потвърди емайл адрес", callbackUrl);
                    var count = await _userManager.Users.FirstOrDefaultAsync();//.CountAsync().ConfigureAwait(false);

                    if (count == null)
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
                        if (viewModel.isEmployer)
                        {
                            await _userManager.AddToRoleAsync(user, "Employer");
                            user.Role = Roles.Employer;
                        } 
                        else
                        {
                            await _userManager.AddToRoleAsync(user, "User");
                            user.Role = Roles.User;
                        }
                    }


                    await _notifyService.Create("Моля попълнете личните си данни.", "identity/account/manage/editprofile", DateTime.Now, NotifyType.Information, null, user.Id, null).ConfigureAwait(false);

                    _baseService.ToastNotify(ToastMessageState.Alert, "Детайли", "Моля попълнете личните си данни.", 9000);
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "се регистрирахте. Благодарим ви за отделеното време !", 5000);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = viewModel.Email, /*returnUrl = returnUrl*/ });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToPage("/Account/Manage", new { Area = "Identity" });
                    }

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
                HttpContext.Response.Cookies.Delete("GrandCookie");
            }

            return RedirectToAction("Index", "Home");
        }


        [Route("/social-switch")]
        public async Task<IActionResult> SignInSocialEnabling(string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Моля влезте в системата отново или си изчистете кеш данните на браузъра.");
            }

            if (!user.SignInSocialEnable)
                user.SignInSocialEnable = true;
            else
                user.SignInSocialEnable = false;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        [Route("/email-switch")]
        public async Task<IActionResult> EmailNotifyEnabling(string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Моля влезте в системата отново или си изчистете кеш данните на браузъра.");
            }

            if (!user.EmailNotifyEnable)
                user.EmailNotifyEnable = true;
            else
                user.EmailNotifyEnable = false;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }


        // User Modifications
        [Authorize(Roles = "Admin")]
        [Route("/exchange-role")]
        public async Task<ActionResult> ExchangeRole(string id, Roles T, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("Identity/Account/Errors/AccessDenied");
            }

            var user_2 = await _userManager.FindByIdAsync(id);
            if (user_2 == null)
            {
                return Redirect("Identity/Account/Errors/AccessDenied");
            }

            string roleName = user_2.Role.GetShortName();
            await _userManager.RemoveFromRoleAsync(user_2, roleName);

            switch (T)
            {
                case Roles.Admin:
                    {
                        roleName = "Admin";
                        user_2.Role = Roles.Admin;
                    }
                    break;
                case Roles.Moderator:
                    {
                        roleName = "Moderator";
                        user_2.Role = Roles.Moderator;
                    }
                    break;
                case Roles.Employer:
                    {
                        roleName = "Employer";
                        user_2.Role = Roles.Employer;
                    }
                    break;
                case Roles.Contestant:
                    {
                        roleName = "Contestant";
                        user_2.Role = Roles.Contestant;
                    }
                    break;
                case Roles.Recruiter:
                    {
                        roleName = "Recruiter";
                        user_2.Role = Roles.Recruiter;
                    }
                    break;
                case Roles.User:
                    {
                        roleName = "User";
                        user_2.Role = Roles.User;
                    }
                    break;
            }

            await _userManager.AddToRoleAsync(user_2, roleName);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToPage("/Account/Manage/Users", new { Area = "Identity" });
        }

        [Route("/remove-usr")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string id, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("Identity/Account/Errors/AccessDenied");
            }

            var user_2 = await _userManager.FindByIdAsync(id);
            if (user_2 == null)
            {
                return Redirect("Identity/Account/Errors/AccessDenied");
            }

            _baseService.DeleteUserResources(user_2, true);

            await _jobsService.DeleteAllBy(0, user_2);
            await _companyService.DeleteAllBy(user_2);
            await _contestantService.DeleteAllBy(user_2);
            await _resumeService.DeleteAllBy(user_2);

           var result = await _userManager.DeleteAsync(user_2);

            if (!result.Succeeded)
                await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "при изтриване на потребител.","", 2000);
            else
                await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Success, "Успешно", "е изтрит потребителят.", "", 2000);

            if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                return RedirectToPage("/Account/Manage/Users", new { Area = "Identity" });

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
