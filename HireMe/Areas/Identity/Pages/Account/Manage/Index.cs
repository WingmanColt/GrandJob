using HireMe.Entities;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Core;
using HireMe.Services.Core.Interfaces;
using HireMe.Services.Interfaces;
using HireMe.StoredProcedures.Interfaces;
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
        private readonly IContestantsService _contestantService;
        private readonly IJobsService _jobsService;
        private readonly IspJobService _spJobsService;
        private readonly ICompanyService _companyService;
        //private readonly IMessageService _messageService;
        private readonly IFavoritesService _favouritesService;
        private readonly IResumeService _resumeService;
        private readonly IFilesService _filesService;
        private readonly ITaskService _taskService;

       // private readonly IStatisticsService _statsService;
        private readonly INotificationService _notificationService;
        private readonly ICipherService _cipherService;

        public IndexModel(UserManager<User> userManager,
            IBaseService baseService,
            IContestantsService contestantService,
            IJobsService jobsService,
            IspJobService spJobService,
            ICompanyService companyService,
           // IMessageService messageService,
            IFavoritesService favouritesService,
            IResumeService resumeService,
            IFilesService filesService,
            ITaskService taskService,
         //   IStatisticsService statsService,
            INotificationService notificationService,
            ICipherService cipherService)
        {
            _userManager = userManager;
            _baseService = baseService;
            _contestantService = contestantService;
            _jobsService = jobsService;
            _spJobsService = spJobService;

            _companyService = companyService;
             _favouritesService = favouritesService;

            _resumeService = resumeService;
            _filesService = filesService;
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

        public int MyAppliedJobsCount { get; set; }
        public IAsyncEnumerable<Entities.Models.Jobs> MyAppliedJobs { get; set; }

        // Messages & Apps
        public User LoggedUser { get; set; }
        public int MyMessages { get; set; }
        public int MyAppsCount { get; set; }
        public int TasksCount { get; set; }
        public IList<UserLoginInfo> MyApps { get; set; }
        public IAsyncEnumerable<Entities.Models.Resume> ReceivedResumes { get; set; }
        public IAsyncEnumerable<Entities.Models.Jobs> LastJobs { get; set; }

        // Notify
        public IAsyncEnumerable<Notification> Notifications { get; set; }
        public int NotifyCount { get; set; }
        public bool isNotiftEmpty { get; set; }

        //Chart
        public IAsyncEnumerable<SelectListModel> SelectCompanyChart { get; set; }
        public IAsyncEnumerable<SelectListModel> SelectJobChart { get; set; }

        [BindProperty]
        public TaskInputModel TaskInput { get; set; }


        [BindProperty]
        public ResumeInput RInput { get; set; }
        public class ResumeInput : CreateResumeInputModel 
        {
            public int resumeId { get; set; }
            public int Rating { get; set; } = 0;
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

            switch (user.Role)
            {
                case Roles.User:case Roles.Contestant:
                    TasksCount = await _taskService.GetAllCount(user).ConfigureAwait(false);
                    MyContestant = await _contestantService.GetCountByUser(user).ConfigureAwait(false);
                    MyAppliedJobs =  _favouritesService.GetFavouriteBy<Entities.Models.Jobs>(user, PostType.Job);
                    MyAppliedJobsCount = (int)await _favouritesService.GetFavouriteByCount(user, PostType.Job).ConfigureAwait(false);
                    break;                       
                case Roles.Recruiter:case Roles.Employer: case Roles.Admin:
                    TasksCount = await _taskService.GetAllCount(user).ConfigureAwait(false);
                    ReceivedResumes = await _jobsService.GetAllReceivedResumes(user, ResumeType.Active);
                    MyJobs = await _spJobsService.GetAllCountBy(new { PosterId = user.Id}).ConfigureAwait(false);
                    MyCompanies = await _companyService.GetCountByUser(user).ConfigureAwait(false);
                    SelectCompanyChart = _companyService.GetAllSelectList(user);
                    var jobResult = await _spJobsService.GetAll<Entities.Models.Jobs>(new { PosterId = user.Id }, StoredProcedures.Enums.JobGetActionEnum.GetAllBy, false, null);
                    SelectJobChart = jobResult.Select(x => new SelectListModel
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name.ToString()
                    });
                    break;
                //case Roles.Moderator: case Roles.Admin:
                  //  break;

            }

            MyAppsCount = MyApps is null ? 0 : MyApps.Count();
            MyMessages = 0;//await _messageService.GetMessagesCountBy_Receiver(user.UserName);


            // Notify
            Notifications = _notificationService.GetAllBy(user);
            NotifyCount = await _notificationService.GetNotificationsCount(user);
            isNotiftEmpty = Notifications is null ? false : await Notifications.AnyAsync();

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

            //use thiss !
            //var jobIdsString = await _statsService.GetByIdAsync(user.Id).ConfigureAwait(true);
            //MyAppliedJobs = jobIdsString is not null ? _jobsService.GetAllByStats(jobIdsString.AppliedJobsId) : null;

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
                await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Success, "Успешно", "добавихте задачата си", "#", 3000).ConfigureAwait(false);
                if (receiverUser is not null)
                {
                    await _notificationService.Create($"Успешно създадохте вашата задача с {receiverUser.FirstName} {receiverUser.LastName}.", "identity/tasks/index", DateTime.Now, NotifyType.Tasks, null, user.Id, null).ConfigureAwait(false);
                    await _notificationService.Create("Вие получихте нова задача току-що.", "identity/tasks/index", DateTime.Now, NotifyType.Tasks, null, receiverUser.Id, user.Id).ConfigureAwait(false);
                }

            }
            else
                await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", result.FailureMessage, "#", 6000).ConfigureAwait(false);


            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRateAsync([FromServices] ISenderService _senderService)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("AccessDenied", new { Area = "Errors" });
            }

            var resumeEntity = await _resumeService.GetByIdAsync(RInput.resumeId);

            var filesEntity = await _filesService.GetByLastAppliedJob(RInput.LastAppliedJob);
            if (filesEntity is not null)       
                await _filesService.AddRating(filesEntity, (double)RInput.Rating);
            

            if (resumeEntity is not null)
            {
                await _resumeService.AddRating(resumeEntity, (double)RInput.Rating);
                if(resumeEntity.isGuest)
                    await _senderService.SendEmailAsync(resumeEntity.UserId, $"{resumeEntity.LastAppliedJob} - Оцени твоето CV с {RInput.Rating} / 5", "https://grandjob.eu/");

                await _resumeService.Archive(resumeEntity);

                var userId = await _userManager.FindByEmailAsync(resumeEntity.UserId);   

                if(userId is not null)
                await _notificationService.Create($"{resumeEntity.LastAppliedJob} оцени вашето CV с {RInput.Rating}/5.", "identity/resume/index", DateTime.Now, NotifyType.Information, null, userId.Id, null).ConfigureAwait(false);

            }

            return RedirectToPage();
        }
    }
}
