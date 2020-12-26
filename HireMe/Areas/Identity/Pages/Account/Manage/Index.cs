using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using HireMe.ViewModels.Favorites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    [ResponseCache(CacheProfileName = "Weekly")]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IContestantsService _contestantService;
        private readonly IJobsService _jobsService;
        private readonly ICompanyService _companyService;
        private readonly IMessageService _messageService;
        private readonly IFavoritesService _favouritesService;
        private readonly IResumeService _resumeService;
        public IndexModel(UserManager<User> userManager,
            IContestantsService contestantService,
            IJobsService jobsService,
            ICompanyService companyService,
            IMessageService messageService,
            IFavoritesService favouritesService,
            IResumeService resumeService)
        {
            _userManager = userManager;
            _contestantService = contestantService;
            _jobsService = jobsService;
            _companyService = companyService;
            _messageService = messageService;
            _favouritesService = favouritesService;
            _resumeService = resumeService;
        }

        // Admin
        public int AllJobsWaiting { get; set; }
        public int AllCompaniesWaiting { get; set; }
        public int AllContestantsWaiting { get; set; }

        // Employer & Recruiter
        public int MyJobs { get; set; }
        public int MyCompanies { get; set; }
        public int MyContestant { get; set; }
        public int MyAppliedJobs { get; set; }

        // Messages & Apps
        public int MyMessages { get; set; }
        public int MyAppsCount { get; set; }
        public IList<UserLoginInfo> MyApps { get; set; }
        public IAsyncEnumerable<Entities.Models.Resume> ReceivedResumes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            if (user.AccountType == 0)
            {
                return RedirectToPage("Pricing");
            }

            if(user.Role.Equals(Roles.Employer) || user.Role.Equals(Roles.Recruiter))
            {
                var job = await _jobsService.GetAllAsNoTracking()
             .Where(x => x.isApproved == ApproveType.Success && (x.PosterID == user.Id))
             .AsQueryable()
             .FirstOrDefaultAsync();

             ReceivedResumes = _resumeService.GetAllReceived(job);
            }

            //MyJobs = await _jobsService.GetAllCountByCondition(-1, -1, user.UserName, ApproveType.Success);
            /*
            // Admin
            AllJobsWaiting = await _jobsService.GetAllCountByCondition(-1, -1, null, ApproveType.Waiting, false);
            AllCompaniesWaiting = await _companyService.GetAllCountByCondition(ApproveType.Waiting, null);
            AllContestantsWaiting = await _contestantService.GetAllCountByCondition(-1, null, ApproveType.Waiting, false);

            // Employer & Recruiter
            MyJobs = await _jobsService.GetAllCountByCondition(-1, -1, user.UserName, ApproveType.Success, false);
            MyCompanies = await _companyService.GetAllCountByCondition(ApproveType.Success,user.UserName);
            MyContestant = await _contestantService.GetAllCountByCondition(-1, user.UserName, ApproveType.Success, false);

            // Messages & Apps
            MyMessages = await _messageService.GetMessagesCountBy_Receiver(user.UserName);
            MyAppliedJobs = await _favouritesService.GetFavouriteByCount(user, PostType.Job);
            MyAppsCount =  MyApps is null ? 0 : MyApps.Count();
            */
            //ViewData["Labels"] = labels;
            //ViewData["Data"] = labels;
            return Page();
        }
    }
}
