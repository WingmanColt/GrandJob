using HireMe.Entities.Models;
using HireMe.Services;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogService _logService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public NotificationsController(
            INotificationService notificationService, 
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogService logService)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logService = logService;
        }

        public async Task<IActionResult> Enabling(string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            if (!user.NotifyEnable)
                user.NotifyEnable = true;
            else
                user.NotifyEnable = false;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> ClearNotifications(string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            await this._notificationService.RemoveAllBy(user);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> ClearLog(int id, string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            await _logService.Delete(id);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> ClearAllLogs(string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            await _logService.DeleteAll();

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
    }
}