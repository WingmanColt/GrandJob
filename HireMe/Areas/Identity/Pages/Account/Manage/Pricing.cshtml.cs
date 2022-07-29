using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class PricingModel : PageModel
    {
        private readonly IBaseService _baseService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public PricingModel(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IBaseService baseService,
            INotificationService notificationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _baseService = baseService;
            _notificationService = notificationService;
        }

        public User UserEntity { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;
            return Page();
        }

        public async Task<IActionResult> OnPostFreeAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            if (user.AccountType != AccountType.Free)
            {
                user.AccountType = AccountType.Free;

                _baseService.ToastNotify(ToastMessageState.Alert, "Детайли", "Моля попълнете личните си данни.", 10000);
                await _notificationService.Create("Успешно активирахте избрания от вас пакет.", "#", DateTime.Now, NotifyType.Success, null, user.Id, null).ConfigureAwait(false);
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToPage("Index");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostProAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            if (user.AccountType != AccountType.Pro)
            {
                user.AccountType = AccountType.Pro;
                _baseService.ToastNotify(ToastMessageState.Alert, "Детайли", "Моля попълнете личните си данни.", 10000);

                await _notificationService.Create("Успешно активирахте избрания от вас пакет.", "#", DateTime.Now, NotifyType.Success, null, user.Id, null).ConfigureAwait(false);

                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToPage("Index");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostTestAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            if (user.AccountType != AccountType.Test)
            {
                user.AccountType = AccountType.Test;
                _baseService.ToastNotify(ToastMessageState.Alert, "Детайли", "Моля попълнете личните си данни.", 10000);

                await _notificationService.Create("Успешно активирахте избрания от вас пакет.", "#", DateTime.Now, NotifyType.Activated, null, user.Id, null).ConfigureAwait(false);

                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}