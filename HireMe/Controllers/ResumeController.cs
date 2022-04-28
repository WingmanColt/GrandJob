using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    public class ResumeController : BaseController
    {
        private readonly IBaseService _baseService;
        private readonly IJobsService _jobsService;
        private readonly IResumeService _resumeService;
        private readonly UserManager<User> _userManager;

        private readonly string _FilePath;

        public ResumeController(
            IConfiguration config,
            IBaseService baseService,
            IJobsService jobsService,
            IResumeService resumeService,
            UserManager<User> userManager)
        {
            _baseService = baseService;
            _jobsService = jobsService;
            _resumeService = resumeService;
            _userManager = userManager;

            _FilePath = config.GetValue<string>("MySettings:UploadsPathHosting");
        }


        [Authorize]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

         //   var jobTitle = await _resumeService.GetByIdAsync(id);
            var result = await _jobsService.RemoveResumeFromReceived(id);

            if(!result.Success)
            _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "", result.FailureMessage, 6000);
            

          return RedirectToPage("/Identity/Index", new { Area = "Identity" });
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetResume(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            
            var entity = await _resumeService.GetByIdAsync(id);
            if (entity == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }
            string dir = _FilePath + "\\" + (entity.isGuest == true ? "JobsCV" : "CV");
            string folderName = (entity.isGuest == true ? StringHelper.Filter(entity.LastAppliedJob) : StringHelper.Filter(entity.UserId.GetUntilOrEmpty("@")));

            var file = $"{dir}\\{folderName}\\{entity.FileId}";

            // Response...
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {

                FileName = "GrandJob | Resume Preview",
                Inline = false  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");


           return new PhysicalFileResult(file, "application/pdf");
        }
    }
}
