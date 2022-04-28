using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Core;
using HireMe.Services.Core.Interfaces;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    //[ResponseCache(CacheProfileName = "Weekly")]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IBaseService _baseService;
        //private readonly IContestantsService _contestantService;
        private readonly IJobsService _jobsService;
       // private readonly ICompanyService _companyService;
        //private readonly IMessageService _messageService;
        //private readonly IFavoritesService _favouritesService;
        private readonly IResumeService _resumeService;
        private readonly ITaskService _taskService;

        private readonly IStatisticsService _statsService;
        private readonly INotificationService _notificationService;
        private readonly ICipherService _cipherService;

        public IndexModel(UserManager<User> userManager,
            IBaseService baseService,
            //IContestantsService contestantService,
            IJobsService jobsService,
            //ICompanyService companyService,
           // IMessageService messageService,
           // IFavoritesService favouritesService,
            IResumeService resumeService,
            ITaskService taskService,
            IStatisticsService statsService,
            INotificationService notificationService,
            ICipherService cipherService)
        {
            _userManager = userManager;
            _baseService = baseService;
            //_contestantService = contestantService;
            _jobsService = jobsService;
            //_companyService = companyService;
            //_messageService = messageService;
            // _favouritesService = favouritesService;

            _statsService = statsService;
            _resumeService = resumeService;
            _taskService = taskService;
            _notificationService = notificationService;
            _cipherService = cipherService;
        }

        // Admin
        public int AllJobsWaiting { get; set; }
        public int AllCompaniesWaiting { get; set; }
        public int AllContestantsWaiting { get; set; }

        // Employer & Recruiter
        public int MyJobs { get; set; }
        public int MyCompanies { get; set; }
        public int MyContestant { get; set; }
        public IAsyncEnumerable<Entities.Models.Jobs> MyAppliedJobs { get; set; }

        // Messages & Apps
        public User LoggedUser { get; set; }
        public int MyMessages { get; set; }
        public int MyAppsCount { get; set; }
        public IList<UserLoginInfo> MyApps { get; set; }
        public IAsyncEnumerable<Entities.Models.Resume> ReceivedResumes { get; set; }
        public IAsyncEnumerable<Entities.Models.Jobs> LastJobs { get; set; }

        // Notify
        public IAsyncEnumerable<Notification> Notifications { get; set; }
        public int NotifyCount { get; set; }
        public bool isNotiftEmpty { get; set; }

        [BindProperty]
        public TaskInputModel TaskInput { get; set; }


        [BindProperty]
        public ResumeInput RInput { get; set; }
        public class ResumeInput : CreateResumeInputModel 
        {
            public int resumeId { get; set; }
            public int Rating { get; set; }
            public int JobId { get; set; }
        }


        //public User ReceiverUser { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.AccountType is 0)
            {
                return RedirectToPage("Pricing");
            }

            LoggedUser = user;

            if (user.Role.Equals(Roles.Employer) || user.Role.Equals(Roles.Recruiter) || user.Role.Equals(Roles.Admin))
            {
                RInput = new ResumeInput
                {
                    Rating = 0
                };


                var jobs = await _jobsService.GetAllAsNoTracking()
                .Where(x => x.PosterID == user.Id)
                .AsQueryable()
                .ToListAsync();

                var list = new List<Entities.Models.Resume>();
                IAsyncEnumerable<Entities.Models.Resume> resumes;

                foreach (var item in jobs)
                {
                    resumes = _resumeService.GetAllReceived(item);

                    await foreach (var res in resumes)
                    {
                        list.Add(res);
                    }

                }

                ReceivedResumes = list.ToAsyncEnumerable();           
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

            // Notify
            Notifications = _notificationService.GetAllBy(user);
            NotifyCount = await _notificationService.GetNotificationsCount(user);
            isNotiftEmpty = Notifications is null ? false : await Notifications.AnyAsync();

            var jobIdsString = await _statsService.GetByIdAsync(user.Id).ConfigureAwait(true);
            MyAppliedJobs = jobIdsString is not null ? _jobsService.GetAllByStats(jobIdsString.AppliedJobsId) : null;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var receiverUser = TaskInput.ReceiverId is not null ? await _userManager.FindByEmailAsync(TaskInput.ReceiverId) : null;

            if (user == null)
            {
                return RedirectToPage("AccessDenied", new { Area = "Errors" });
            }
            LoggedUser = user;

            TaskInput.SenderId = user.Id;
            TaskInput.ReceiverId = TaskInput.ReceiverId;
            TaskInput.GeneratedLink = _cipherService.Encrypt(user.Id + TaskInput.ReceiverId);
            TaskInput.Status = TasksStatus.Waiting;

            var result = await _taskService.Create(TaskInput);

            if (result.Success)
            {
                _baseService.ToastNotifyLogAsync(user, ToastMessageState.Success, "Успешно", "добавихте задачата си", "#", 3000);
                if (receiverUser is not null)
                {
                    await _notificationService.Create($"Успешно създадохте вашата задача с {receiverUser.FirstName} {receiverUser.LastName}.", "identity/tasks/index", DateTime.Now, NotifyType.Success, "flaticon-share-1", user.Id, null).ConfigureAwait(false);
                    await _notificationService.Create("Вие получихте нова задача току-що.", "identity/tasks/index", DateTime.Now, NotifyType.Information, "flaticon-share-1", receiverUser.Id, user.Id).ConfigureAwait(false);
                }

            }
            else
                _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", result.FailureMessage, "#", 6000);


            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRateAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("AccessDenied", new { Area = "Errors" });
            }

            var resumeEntity = await _resumeService.GetByIdAsync(RInput.resumeId);

            if (resumeEntity is not null)
            {
                await _resumeService.AddRating(resumeEntity, (double)RInput.Rating);

                var userId = await _userManager.FindByEmailAsync(resumeEntity.UserId);   

                if(userId is not null)
                await _notificationService.Create($"{user.FirstName} {user.LastName} оцени вашето CV с {RInput.Rating}/5.", "identity/resume/index", DateTime.Now, NotifyType.Warning, "flaticon-star", userId.Id, null).ConfigureAwait(false);

              //  await _resumeService.Delete(resumeEntity);


            }

            return RedirectToPage();
        }
    }
}
