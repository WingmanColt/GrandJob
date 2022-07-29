namespace HireMe.Areas.Identity.Pages.Checkout
{
    using HireMe.Entities.Models;
    using HireMe.Payments;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    public partial class SuccessModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly string _clientID;
        private readonly string _clientSecret;

        private readonly PaypalClient _paypalClient;

        public SuccessModel(
            UserManager<User> userManager,
            IConfiguration config,
            PaypalClient paypalClient)
        {
            _userManager = userManager;

            _paypalClient = paypalClient;
            _clientID = config.GetValue<string>("PayPal:ClientId");
            _clientSecret = config.GetValue<string>("PayPal:ClientSecret");
        }


        public User UserEntity { get; set; }

            public async Task<IActionResult> OnGetAsync(string token)
            {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            var accesstoken = await _paypalClient.GetToken(_clientID, _clientSecret);
            if (accesstoken != null)
            {
             //   var result = await _paypalClient..CaptureOrder(accesstoken, token);
             //   if (result)
            //    {
                    return Page();
             //   }
            }

            return Redirect("~/Identity/Account/Manage/Index");

        }

    }
}