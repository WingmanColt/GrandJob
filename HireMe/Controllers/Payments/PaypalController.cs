using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace HireMe.Controllers.Payments
{
    public class PaypalController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly IConfiguration _config;
        private readonly IPromotionService _promotionService;
        public PaypalController(UserManager<User> userManager, IConfiguration config, IPromotionService promotionService)
        {
            _userManager = userManager;
            _config = config;
            _promotionService = promotionService;
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employer, Contestant")]
        public async Task<IActionResult> PaymentSuccessful(string id, PromotionInput input)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = _userManager.GetUserId(User);

            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }

            if (userId != id)
            {
                return NotFound($"Unable to load user.");
            }

            await _promotionService.Create(input.Id, id, PromotionEnum.AccountUpgrade, DateTime.Now, DateTime.Now.AddDays(30.0));

            //   return View();
            return Redirect($"/identity/account/manage/");
        }


    }
}
