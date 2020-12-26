using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    [Authorize]
    public class ResumeController : BaseController
    {
        private readonly IBaseService _baseService;
        private readonly IJobsService _jobsService;
        private readonly UserManager<User> _userManager;

        private readonly string _FilePath;
        private readonly string[] _permittedFiles = { "application/pdf", ".ps", ".doc", ".docx", ".odt", ".sxw", ".txt", ".rtf" };

        public ResumeController(
            IConfiguration config,
            IBaseService baseService,
            IJobsService jobsService,
            UserManager<User> userManager)
        {
            _baseService = baseService;
            _jobsService = jobsService;
            _userManager = userManager;

            _FilePath = config.GetValue<string>("StoredFilesPath");
        }


        public async Task<IActionResult> DeleteApplication(int id, string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }
            var job = await _jobsService.GetAll()
                .Where(x => x.PosterID == user.Id)
                .FirstOrDefaultAsync();

            var result = await _jobsService.RemoveResumeFromReceived(id.ToString(), job);

            if(!result.Success)
            _baseService.ToastNotifyLog(user, ToastMessageState.Error, "Грешка", "Resume/DeleteApplication", result.FailureMessage, 6000);
            
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetResume(string id)
        {
            return new PhysicalFileResult($"{_FilePath}\\{id}", "application/pdf");
        }
    }
}
