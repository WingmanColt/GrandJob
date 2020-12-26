using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using HireMe.ViewModels.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.ViewComponents
{
    [ViewComponent(Name = "_Activity")]
    public class _ActivityViewComponent : ViewComponent
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;

        private readonly INotificationService _notifyService;
        private readonly IMessageService _messageService;
        private readonly IFavoritesService _favouriteService;

        public _ActivityViewComponent(
            IConfiguration config,
            UserManager<User> userManager,
            INotificationService notifyService,
            IMessageService messageService,
            IFavoritesService favouriteService)
        {
            _config = config;
            _userManager = userManager;
            _notifyService = notifyService;
            _messageService = messageService;
            _favouriteService = favouriteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new ActivityViewModel();
            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);

            if (user is not null)
            {
                model.User = user;
                model.FullName = user.FirstName?.Substring(0, 1) + user.LastName?.Substring(0, 1);
                model.SiteUrl = user.isExternal ? null : _config.GetSection("MySettings").GetSection("SiteImageUrl").Value;

                // Messages
                model.Messages = _messageService.GetMessagesBy(user, MessageClient.Receiver, 10);
                model.MessagesCount = await _messageService.GetMessagesCountBy_Receiver(user);
                model.isMessagesEmpty = model.Messages is null ? false : await model.Messages.AnyAsync();

                // Notify
                model.Notifications = _notifyService.GetAllBy(user);
                model.NotifyCount = await _notifyService.GetNotificationsCount(user);
                model.isNotiftEmpty = model.Notifications is null ? false : await model.Notifications.AnyAsync();

                // Favourites
                model.FavouriteJob = _favouriteService.GetFavouriteBy<Jobs>(user, PostType.Job);
                model.FavouriteContestant = _favouriteService.GetFavouriteBy<Contestant>(user, PostType.Contestant);
                model.FavouriteCompany = _favouriteService.GetFavouriteBy<Company>(user, PostType.Company);
                model.FavouritesCount = await _favouriteService.GetFavouriteByCount(user, PostType.All);

                model.ReturnUrl = Url.PageLink();
            }
            return View(model);           
        }

    }
}