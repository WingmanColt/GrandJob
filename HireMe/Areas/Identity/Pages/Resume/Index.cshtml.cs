namespace HireMe.Areas.Identity.Pages.Resume
{
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IResumeService _resumeService;
        private readonly IJobsService _jobsService;
        private readonly IBaseService _baseService;
        private readonly string _FilePath;

        public string _SiteUrl;
        public string _resumeUploadLimit;

        public IndexModel(
            IConfiguration config,
            UserManager<User> userManager,
            IResumeService resumeService,
            IJobsService jobsService,
            IBaseService baseService)
        {
            _userManager = userManager;
            _resumeService = resumeService;
            _baseService = baseService;
            _jobsService = jobsService;

            _SiteUrl = config.GetValue<string>("MySettings:ResumeUrl"); 
            _resumeUploadLimit = config.GetValue<string>("ResumeUploadLimit");
            _FilePath = config.GetValue<string>("MySettings:FilePathHosting");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : CreateResumeInputModel { }
        public IAsyncEnumerable<Resume> Result { get; set; }
        public IAsyncEnumerable<Resume> ResultArchive { get; set; }
        public int Count { get; set; }

        public User UserEntity { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.AccountType == 0)
            {
                return RedirectToPage("/Account/Manage/Pricing", new { Area = "Identity" });
            }


            UserEntity = user;

            if (user.Role.Equals(Roles.Contestant) || user.Role.Equals(Roles.User))
            {
                Result = _resumeService.GetAllBy(user);
                Count = await _resumeService.GetAllCount(user).ConfigureAwait(false);
            }

            if (user.Role.Equals(Roles.Employer) || user.Role.Equals(Roles.Recruiter))
            {
                Result = await _jobsService.GetAllReceivedResumes(user, ResumeType.Active);
                ResultArchive = await _jobsService.GetAllReceivedResumes(user, ResumeType.Archived);

                //Count = ;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            /*var entity = await _resumeService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }*/

            /*if (user.Email != entity.UserId)
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }*/

           // string entId = entity.FileId;
            OperationResult result = await _resumeService.Delete(id);

          //   string folderClearedName = Path.Combine(_FilePath, StringHelper.Filter(user?.Email));
           //  bool systemDeleted =  _baseService.Delete($"{folderClearedName}\\{entId}");

            if (result.Success /*&& systemDeleted*/)
                    _baseService.ToastNotify(ToastMessageState.Info, "", "файлът е премахнат.", 2000);
                else
                    await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "", "файлът не се изтри.","Resume/Index", 5000);

            return RedirectToPage();
        }

    }
}