using HireMe.Entities;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Mapping.Utility;
using HireMe.Services.Interfaces;
using HireMe.ViewModels.Jobs;
using HireMe.ViewModels.Skills;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    public class JobsController : BaseController
    {
        private readonly UserManager<User> _userManager;

        private readonly ICategoriesService _categoriesService;
        private readonly IJobsService _jobsService;
        private readonly ILocationService _locationService;
        private readonly ISkillsService _skillsService;
        private readonly ILanguageService _langService;


        public JobsController(
            UserManager<User> userManager,
            IJobsService jobsService,
            ICategoriesService categoriesService,
            ILocationService locationService,
            ISkillsService skillsService,
            ILanguageService langService)
        {
            _userManager = userManager;
            _jobsService = jobsService;
            _locationService = locationService;
            _skillsService = skillsService;
            _langService = langService;
            _categoriesService = categoriesService;
        }

        [HttpGet]
        [Route("jobs/all")]
        public async Task<IActionResult> Index(int currentPage = 1, string Sort = null)
        {
            var viewModel = new JobsViewModel
            {
                AllLocations = _locationService.GetAllSelectList(),
                AllCategories = _categoriesService.GetAllSelectList()
            };


            var entity = _jobsService.GetAllAsNoTracking()
                .Where(x => (x.isApproved == ApproveType.Success) && !x.isArchived)
                .Select(x => new Jobs
                {
                        Id = x.Id,
                        Name = x.Name,
                        WorkType = x.WorkType,
                        LocationId = x.LocationId,
                        CreatedOn = x.CreatedOn,
                        PosterID = x.PosterID,
                        RatingVotes = x.RatingVotes,
                        Promotion = x.Promotion,
                        Adress = x.Adress,
                        JobType = x.JobType,
                        MinSalary = x.MinSalary,
                        MaxSalary = x.MaxSalary,
                        SalaryType = x.SalaryType,
                        CompanyId = x.CompanyId
                });

            

            switch (Sort)
            {
                case "Рейтинг":
                    entity = entity.OrderByDescending(x => x.RatingVotes);
                    break;
                case "Последни":
                    entity = entity.OrderBy(x => x.CreatedOn);
                    break;
                case "Най-нови":
                    entity = entity.OrderByDescending(x => x.CreatedOn);
                    break;
                case "Заплата":
                    entity = entity.OrderBy(x => x.MaxSalary);
                    break;
                default:
                    entity = entity.OrderByDescending(x => x.CreatedOn);
                    break;
            }


            if (await entity.AnyAsync()) // prevent 'SqlException: The offset specified in a OFFSET clause may not be negative.'
            {
                int count = await entity.AsQueryable().AsNoTracking().CountAsync().ConfigureAwait(false);
                viewModel.Pager = new Pager(count, currentPage);

                var result = entity
                .Skip((viewModel.Pager.CurrentPage - 1) * viewModel.Pager.PageSize)
                .Take(viewModel.Pager.PageSize);

                viewModel.Result = result.To<JobsViewModel>().AsAsyncEnumerable();
            }
            else viewModel.Result = null;

            return View(viewModel);
        }

        [Route("jobs/new")]
        public async Task<IActionResult> Create([FromServices] IBaseService _baseService)
        {   
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }
            if (user.AccountType == 0)
            {
                return Redirect("/Identity/Account/Manage/Pricing");
            }
            if (!user.profileConfirmed)
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "Моля попълнете личните си данни преди да продължите.", 7000);
                return Redirect($"/Identity/Account/Manage/EditProfile");
            }

            CreateJobInputModel viewModel = new CreateJobInputModel();
            viewModel.AllLocations = _locationService.GetAllSelectList();
            viewModel.AllCategories = _categoriesService.GetAllSelectList();
           
            return this.View(viewModel);
        }

        [HttpPost]
        [Route("jobs/new")]
        [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
        public async Task<IActionResult> Create(CreateJobInputModel input, [FromServices] IBaseService _baseService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            input.AllLocations = _locationService.GetAllSelectList();
            input.AllCategories = _categoriesService.GetAllSelectList();

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            input.isArchived = false;
            var result = await _jobsService.Create(input, user);

            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Warning, "Внимание", "Моля изчакайте заявката ви да се прегледа от администратор.", 7000);
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Обявата ви е добавена.", 5000);
                return Redirect($"/identity/Jobs/Index");
            }

            return this.View(input);
        }
        [AllowAnonymous]
        [Route("jobs/info")]
        public async Task<IActionResult> Details(int id, [FromServices] IResumeService _resumeService)
        {
            var job = await _jobsService.GetByIdAsyncMapped(id);
            if (job == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            job.SkillsMapped = _skillsService.GetAll<SkillsViewModel>(job.TagsId, true);
          //  job.LanguagesMapped = _langService.GetAllMapped(job.LanguageId);

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                string[] items;
                if (!(job.resumeFilesId is null))
                {
                    items = job.resumeFilesId?.Split(',');

                    job.ResumeFiles = _resumeService.GetAllAsNoTracking()
                    .Where(x => x.UserId == user.Id)
                    .Where(x => !(((IList)items).Contains(x.Id.ToString())))
                    .Select(x => new SelectListModel
                    {
                        Value = x.Id.ToString(),
                        Text = x.Title,
                    }).ToAsyncEnumerable();
                }
                
            }

            await _jobsService.AddRatingToJobs(id, 0.5);
            return this.View(job);
        }

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Approve(int id, ApproveType T, string returnUrl, [FromServices] INotificationService _notifyService, [FromServices] IBaseService _baseService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var entity = await _jobsService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var result = await _baseService.Approve(id, PostType.Job, T);

            if (result.Success)
            {
                var PosterId = await _userManager.FindByIdAsync(entity.PosterID);

                if (!(PosterId is null))
                {

                    switch (T)
                    {
                        case ApproveType.Waiting:
                            await _notifyService.Create("Моля редактирайте вашата обява отново с коректни данни.", "identity/contestant/index", DateTime.Now, NotifyType.Warning, "fas fa-sync-alt", PosterId);
                            break;
                        case ApproveType.Rejected:
                            await _notifyService.Create("Последно добавената ви обява е отхвърлена.", "identity/contestant/index", DateTime.Now, NotifyType.Danger, "fas fa-ban", PosterId);
                            break;
                        case ApproveType.Success:
                            await _notifyService.Create("Последно добавената ви обява е одобрена.", "identity/contestant/index", DateTime.Now, NotifyType.Information, "fas fa-check", PosterId);
                            break;
                    }

                }
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    RedirectToPage("/Identity/Jobs", new { Area = "Identity" });
            }

            return View();
        }
        [Authorize]
        public async Task<ActionResult> UpdateFavourite(int id, string returnUrl, [FromServices] IFavoritesService _favoriteService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            if (!(await _favoriteService.isInFavourite(user, PostType.Job, id)))
                await _favoriteService.AddToFavourite(user, PostType.Job, id.ToString());
            else
                await _favoriteService.RemoveFromFavourite(user, PostType.Job, id.ToString());

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ApplyWithResume(int jobId, JobsViewModel viewModel, string returnUrl, [FromServices] INotificationService _notifyService, [FromServices] IBaseService _baseService, [FromServices] IFavoritesService _favoriteService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var result = await _jobsService.AddResumeFile(jobId, viewModel.resumeFilesId);

            if (result.Success)
            {
                await _favoriteService.AddToFavourite(user, PostType.Job, jobId.ToString());
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Кандидатствахте по тази обява.", 3000);
                return Redirect(returnUrl);
            }
            else
            {
                await _notifyService.Create(result.FailureMessage, $"jobs/details/{jobId}", DateTime.Now, NotifyType.Danger, "fas fa-minus-circle", user);
            }
            
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }


    }

}