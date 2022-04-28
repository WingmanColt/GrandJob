using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services;
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

        private readonly IMessageService _messageService;
        private readonly ILogService _logsService;
        private readonly ITaskService _taskService;

        public _ActivityViewComponent(
            IConfiguration config,
            UserManager<User> userManager,
            IMessageService messageService,
            ILogService logsService,
            ITaskService taskService)
        {
            _config = config;
            _userManager = userManager;
            _messageService = messageService;
            _logsService = logsService;
            _taskService = taskService; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new ActivityViewModel();
            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);

            if (user is not null)
            {
                model.User = user;
                model.FullName = user.FirstName?.Substring(0, 1) + user.LastName?.Substring(0, 1);
                model.SiteUrlUsers = user.isExternal ? null : _config.GetSection("MySettings").GetSection("UserPicturePath").Value;
                model.SiteUrlCompanies = _config.GetSection("MySettings").GetSection("CompanyImageUrl").Value;

                // Messages
                model.Messages = _messageService.GetMessagesBy(user, MessageClient.Receiver, 10);
                model.MessagesCount = await _messageService.GetMessagesCountBy_Receiver(user);
                model.isMessagesEmpty = model.Messages is null ? false : await model.Messages.AnyAsync();

                // Logs
                model.Logs = _logsService.GetAll();

                // Tasks
                model.MyTasks = _taskService.GetAll(user, false);
                model.ReceivedTasks = _taskService.GetAll(user, true);

                model.ReturnUrl = Url.PageLink();
            }
            return View(model);           
        }

    }
}