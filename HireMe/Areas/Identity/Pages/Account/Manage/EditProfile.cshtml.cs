using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    [ResponseCache(CacheProfileName = "Weekly")]
    public partial class EditProfileModel : PageModel
    {
        private readonly IBaseService _baseService;
        private readonly ISenderService _senderService;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly string _ImagePathShow;

        public EditProfileModel(
            IConfiguration config,
            IBaseService baseService,
            ISenderService senderService,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _baseService = baseService;
            _senderService = senderService;

            _userManager = userManager;
            _signInManager = signInManager;

            _ImagePathShow = config.GetValue<string>("MySettings:UserPicturePath");
        }

        public bool IsEmailConfirmed { get; set; }
        public User UserEntity { get; set; }
        public string ReturnUrl { get; set; }

        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationScheme> OtherLogins { get; set; }
        public bool ShowRemoveButton { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [EmailAddress]
            [Display(Name = "Емайл")]
            public string Email { get; set; }

            [Display(Name = "Пол")]
            public Gender Gender { get; set; }

            [Required]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Display(Name = "Потребителско име")]
            public string Username { get; set; }

            [Display(Name = "Аватар")]
            public IFormFile FormFile { get; set; }

            public string Picture { get; set; }
        }


        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var email = await _userManager.GetEmailAsync(user);

            Input = new InputModel
            {
                Email = email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Gender = user.Gender,
                Picture = user.PictureName is not null && user.PictureName.Contains("https://") ? user.PictureName : (_ImagePathShow + user.PictureName) //user.PictureName == null ? null : (user.PictureName.Contains("https://") ? null : _ImagePathShow) + user.PictureName
        };

            UserEntity = user;
            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            CurrentLogins = await _userManager.GetLoginsAsync(user);
            ShowRemoveButton = user.PasswordHash != null || CurrentLogins.Count > 1;
            OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("AccessDenied", new { Area = "Errors" });
            }

            UserEntity = user;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = false;
            if (Input.FormFile != null)
            {
                user.PictureName = await _baseService.UploadImageAsync(Input.FormFile, user.PictureName, false, user);
                result = user.PictureName != null ? true : false;
            }
            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
                result = true;
            }
            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
                result = true;
            }
            if (Input.Username != user.UserName)
            {
                user.UserName = Input.Username; 
                result = true;
            }

            if (result)
            {
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);

                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "е актулизирана информацията за вас.", 2000);
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("AccessDenied", new { Area = "Errors" });
            }

            UserEntity = user;

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            await _senderService.SendEmailAsync(email,
                "Потвърди емайл адрес", callbackUrl);

            _baseService.ToastNotify(ToastMessageState.Info, "Информация", "емайлът за потвърждение е изпратен!", 4000);
            return LocalRedirect(returnUrl);
        }


        // External

        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            {
                _baseService.ToastNotify(ToastMessageState.Warning, "Внимание", "приложението не е премахнато.", 2000);
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "премахнахте приложението.", 2000);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Page("./EditProfile", pageHandler: "LinkLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID 'user.Id'.");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
            if (info == null)
            {
                await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Проблем", "със свързването на приложение, моля свържете се с администратор.", $"{user.UserName}, {user.Email} ,{user.Role}", 2000).ConfigureAwait(true);
                return RedirectToPage(); 
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                _baseService.ToastNotify(ToastMessageState.Warning, "Внимание", "приложението не е свързано, само един акаунт можете да свържете.", 2000);
                return RedirectToPage();
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "свързахте приложението.", 2000);
            return RedirectToPage();
        }
    }
}
