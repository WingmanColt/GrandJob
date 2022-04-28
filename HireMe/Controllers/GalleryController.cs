using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    [Authorize]
    public class GalleryController : BaseController
    {
        private readonly IBaseService _baseService;
        private readonly ITaskService _taskService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;

        private readonly string _rootPath;
        public GalleryController(
            IConfiguration config,
            IBaseService baseService,
            ITaskService taskService,
            INotificationService notificationService,
            UserManager<User> userManager)
        {
            _baseService = baseService;
            _taskService = taskService;
            _notificationService = notificationService;
            _userManager = userManager;

            _rootPath = config.GetValue<string>("MySettings:Gallery");
        }

        public IEnumerable<string> GetImage(string companyName)
        {
            if (String.IsNullOrEmpty(companyName))
                return null;

            string fullDir = _rootPath + companyName;
            if (!Directory.Exists(fullDir))
            {
                Directory.CreateDirectory(fullDir);
            }

           return Directory.GetFiles(fullDir + @"\Images\").Select(Path.GetFileName);
        }
    }
}
