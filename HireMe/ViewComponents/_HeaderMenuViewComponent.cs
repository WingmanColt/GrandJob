using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services;
using HireMe.Services.Interfaces;
using HireMe.ViewModels.Components;
using HireMe.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.ViewComponents
{
    [ViewComponent(Name = "_HeaderMenu")]
    public class _HeaderMenuViewComponent : ViewComponent
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;

        private readonly INotificationService _notifyService;
        private readonly IFavoritesService _favouriteService;

        public _HeaderMenuViewComponent(
            IConfiguration config,
            UserManager<User> userManager,
            INotificationService notifyService,
            IFavoritesService favouriteService)
        {
            _config = config;
            _userManager = userManager;
            _notifyService = notifyService;
            _favouriteService = favouriteService;
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IViewComponentResult> InvokeAsync(bool isDashboard)
        {
            var model = new HeaderMenuViewModel();
            User user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);

            model.isDashboard = isDashboard;

            if (user is not null)
            {
                model.User = user;
                model.SiteImageUrl = (user?.PictureName is not null && user.PictureName.Contains("https://")) ? user?.PictureName : (_config.GetSection("MySettings").GetSection("UserPicturePath").Value + user?.PictureName);
                model.Url = _config.GetSection("MySettings").GetSection("SiteUrl").Value;
                model.UserImageUrl = _config.GetSection("MySettings").GetSection("UserPicturePath").Value;
                model.CompanyImageUrl = _config.GetSection("MySettings").GetSection("CompanyImageUrl").Value;

                // Notify
                model.Notifications = _notifyService.GetAllBy(user).Take(10);
                model.NotifyCount = model.Notifications is null ? 0 : await _notifyService.GetNotificationsCount(user).ConfigureAwait(false);
                model.isNotiftEmpty = model.Notifications is null ? false : await model.Notifications.AnyAsync();

                // Favourites
                model.FavouriteJob = _favouriteService.GetFavouriteBy<Jobs>(user, PostType.Job);
                model.FavouriteContestant = _favouriteService.GetFavouriteBy<Contestant>(user, PostType.Contestant);
                model.FavouriteCompany = _favouriteService.GetFavouriteBy<Company>(user, PostType.Company);

                model.FavouritesCount = await _favouriteService.GetFavouriteByCount(user, PostType.All).ConfigureAwait(false);
                model.ReturnUrl = Url.PageLink();


            }
            return View(model);           
        }

    }
}