using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IO;

namespace HireMe.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IBaseService _baseService;
        private readonly IResumeService _resumeService;
        private readonly IFilesService _filesService;
        private readonly UserManager<User> _userManager;

        private readonly string _AppliedCVPath;
        private readonly string _myFilesCVPath;

        public ResumeController(
            IConfiguration config,
            IBaseService baseService,
            IResumeService resumeService,
            IFilesService filesService,
            UserManager<User> userManager)
        {
            _baseService = baseService;
            _resumeService = resumeService;
            _filesService = filesService;
            _userManager = userManager;

            _AppliedCVPath = config.GetValue<string>("CVPaths:AppliedCVPath");
            _myFilesCVPath = config.GetValue<string>("CVPaths:myFilesCVPath");
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
            var result = await _resumeService.Delete(id); //await _jobsService.RemoveResumeFromReceived(id);

            if (!result.Success)
            await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "", result.FailureMessage, 6000).ConfigureAwait(false);
            

          return RedirectToPage("/Identity/account/manage", new { Area = "Identity" });
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetResume(int id)
        {       
            var entity = await _resumeService.GetByIdAsync(id);
            if (entity == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            string folderName = StringHelper.FilterTrimSplit(entity.LastAppliedJob);
            string userEmail = StringHelper.Filter(entity.UserId);

            var file = $"{_AppliedCVPath}\\{folderName}\\{userEmail}\\{entity.FileId}";

            // Response...
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {

                FileName = "GrandJob | Resume Preview",
                Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");


           return new PhysicalFileResult(file, "application/pdf");
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetFile(int id)
        {

            var entity = await _filesService.GetByIdAsync(id);
            if (entity == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            string userEmail = StringHelper.Filter(entity.UserId);
            string file = $"{_myFilesCVPath}\\{userEmail}\\{entity.FileId}";           


            // Response...
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {

                FileName = "GrandJob | Resume Preview",
                Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            
            return new PhysicalFileResult(file, "application/pdf");
        }
    }
}
