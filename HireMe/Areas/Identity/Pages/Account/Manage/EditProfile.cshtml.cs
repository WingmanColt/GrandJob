using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
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

            _ImagePathShow = config.GetValue<string>("MySettings:SiteImageUrl");
        }

        public bool IsEmailConfirmed { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [EmailAddress]
            [Display(Name = "Емайл")]
            public string Email { get; set; }

            [Display(Name = "Пол")]
            public Gender Gender { get; set; }

            //[Required]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            //[Required]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Display(Name = "Потребителско име")]
            public string Username { get; set; }

            [Display(Name = "Аватар")]
            public IFormFile FormFile { get; set; }

            public string Picture { get; set; }
        }


        public async Task<IActionResult> OnGetAsync()
        {
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
                Picture = (user.isExternal ? null : _ImagePathShow) + user.PictureName
            };


            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("AccessDenied", new { Area = "Errors" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool result = false;
            if (Input.FormFile != null)
            {
                user.PictureName = await _baseService.UploadImageAsync(Input.FormFile, user.PictureName, user);
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
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Детайлите са актулизирани.", 2000);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("AccessDenied", new { Area = "Errors" });
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
