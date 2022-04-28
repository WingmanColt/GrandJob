using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ISenderService _senderService;

        public ExternalLoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ISenderService senderService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _senderService = senderService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            [Display(Name = "Аз съм работодател")]
            public bool IsEmployer { get; set; }

            [EmailAddress]
            public string EmailFromSocial { get; set; }

            public string PictureName { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return RedirectToPage("./Manage/Index"); //LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => (c.Type == ClaimTypes.Email) || (c.Type == ClaimTypes.GivenName) || (c.Type == ClaimTypes.Surname) || (c.Type == ClaimTypes.NameIdentifier)))
                {
                    var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    string photo = null;
                    //string _googleApi = _config.GetSection("GoogleConf").GetSection("ID").Value;
                    switch (info.ProviderDisplayName)
                    {
                        case "Facebook": photo = $"https://graph.facebook.com/{identifier}/picture?type=large";
                            break;
                        case "LinkedIn":
                            photo = info.Principal.Claims.ElementAt(4).Value;
                            break;
                    }

                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                        PictureName = photo,
                        
                    };
                    Input.EmailFromSocial = Input.Email;
                    
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new User { UserName = StringHelper.GetUntilOrEmpty(Input.Email, "@"), Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, PictureName = Input.PictureName, Role = ((Roles)(Input.IsEmployer ? 3 : 0)), isExternal = true };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);


                        if (Input.Email != Input.EmailFromSocial)
                        {
                            await _senderService.SendEmailAsync(Input.Email,
                                "Потвърди емайл адрес",
                                $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> <img src='https://i.imgur.com/MOYSeFJ.jpg'> </a> <br>Изпратено с <3 от GrandJob.eu <br>София, Младост 4.");
                        }
                        else
                        {
                            user.profileConfirmed = true;
                            user.EmailConfirmed = true;

                            await _userManager.UpdateAsync(user);
                        }


                        if (_userManager.Users.Count() == 1)
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
                            if (Input.IsEmployer)
                            {
                                await _userManager.AddToRoleAsync(user, "Employer");
                                //user.Role = Roles.Employer;
                            }
                            else
                            {
                                await _userManager.AddToRoleAsync(user, "User");
                                //user.Role = Roles.User;
                            }
                        }

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _userManager.UpdateAsync(user);
                        await _signInManager.RefreshSignInAsync(user);
                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
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
