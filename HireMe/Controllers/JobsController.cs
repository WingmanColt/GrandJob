using HireMe.Core.Helpers;
using HireMe.Entities;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Entities.View;
using HireMe.Mapping.Utility;
using HireMe.Services.Interfaces;
using HireMe.ViewModels.Favorites;
using HireMe.ViewModels.Jobs;
using HireMe.ViewModels.Language;
using HireMe.ViewModels.PartialsVW;
using HireMe.ViewModels.Skills;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    public class JobsController : BaseController
    {
        private readonly UserManager<User> _userManager;

        private readonly ICategoriesService _categoriesService;
        private readonly ILocationService _locationService;

        private readonly ICompanyService _companyService;
        private readonly IJobsService _jobsService;

        private readonly ISkillsService _skillsService;
        private readonly ILanguageService _langService;

        private readonly IStatisticsService _statsService;
        private readonly IFavoritesService _favoriteService;
        private readonly string _GalleryPath;


        public JobsController(
            IConfiguration config,
            UserManager<User> userManager,
            IJobsService jobsService,
            ICompanyService companyService,
            ICategoriesService categoriesService,
            ILocationService locationService,
            ISkillsService skillsService,
            ILanguageService langService,
            IStatisticsService statsService,
            IFavoritesService favoriteService)
        {
            _userManager = userManager;
            _jobsService = jobsService;
            _companyService = companyService;
            _locationService = locationService;
            _skillsService = skillsService;
            _langService = langService;
            _categoriesService = categoriesService;
            _statsService = statsService;
            _favoriteService = favoriteService;

            _GalleryPath = config.GetValue<string>("StoredGalleryPath");
        }

        [HttpGet]
        [Route("jobs/all")]
        public async Task<IActionResult> Index(int currentPage = 1, string SearchString = null, string LocationId = null)
        {
            var user = await _userManager.GetUserAsync(User);

            // var filter = new Filter();
            var viewModel = new JobsViewModel
            {
                AllLocations = _locationService.GetAllSelectList(),
                AllCategories = _categoriesService.GetAllSelectList()            
            };

            viewModel.Skills = _skillsService.GetAll<Skills>(viewModel.TagsId, false);


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
                    CompanyId = x.CompanyId,
                    CategoryId = x.CategoryId,
                    TagsId = x.TagsId,
                    CompanyLogo = x.CompanyLogo,
                    isInFavourites = _favoriteService.isInFavourite(user, PostType.Job, x.Id.ToString())
        });
            if (!String.IsNullOrEmpty(SearchString))
            {
                entity = entity.Where(x => x.Name.Contains(SearchString));
            }
            if (!String.IsNullOrEmpty(LocationId))
            {
                entity = entity.Where(s => s.LocationId.Equals(LocationId));
            }
            if (await entity.AnyAsync()) // prevent 'SqlException: The offset specified in a OFFSET clause may not be negative.'
            {
                int count = await entity.AsQueryable().AsNoTracking().CountAsync().ConfigureAwait(false);
                viewModel.Pager = new Pager(count, currentPage);

                var result = entity
                .Skip((viewModel.Pager.CurrentPage - 1) * viewModel.Pager.PageSize)
                .Take(viewModel.Pager.PageSize);

                // filter.currentPage = currentPage;

                viewModel.Result = result
                    .To<JobsViewModel>()                  
                    .AsAsyncEnumerable();


            }
            else viewModel.Result = null;

            return View(viewModel);
        }

        [HttpPost]
        [Route("jobs/all")]
        public async Task<IActionResult> Index(Filter filter, int currentPage, int categoryid)
        {
            var user = await _userManager.GetUserAsync(User);

            if (currentPage <= 0)
                currentPage = 1;

            var viewModel = new JobsViewModel
            {
                AllLocations = _locationService.GetAllSelectList(),
                AllCategories = _categoriesService.GetAllSelectList()
            };
            viewModel.Skills = _skillsService.GetAll<Skills>(viewModel.TagsId, false);

           // if(filter.Equipments.Capacity > 0)
             //filter.Equipments = Equipments;

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
                    CompanyId = x.CompanyId,
                    CategoryId = x.CategoryId,
                    TagsId = x.TagsId,
                    CompanyLogo = x.CompanyLogo,
                    isInFavourites = _favoriteService.isInFavourite(user, PostType.Job, x.Id.ToString())
                });

            if (!String.IsNullOrEmpty(filter.SearchString))
            {
                entity = entity.Where(x => x.Name.Contains(filter.SearchString));
            }

            if (!String.IsNullOrEmpty(filter.TagsId))
            {
                string[] tags = filter.TagsId?.Split(", ");
                entity = entity.Where(x => ((IList)tags).Contains(x.TagsId));
            }

            if (!String.IsNullOrEmpty(filter.LanguageId))
            {
                string[] langs = filter.LanguageId?.Split(", ");
                entity = entity.Where(x => ((IList)langs).Contains(x.LanguageId));
            }
            if (filter.Equipments?.Capacity > 0)
            {
                foreach (var item in filter.Equipments)
                {
                    if (item.IsChecked)
                        entity = entity.Where(x => x.WorkType.Contains(item.Value));
                }
            }

            if (filter.MinSalary > 0)
            {
              entity = entity.Where(x => x.MinSalary >= filter.MinSalary);
            }
            if (filter.MaxSalary < 100000)
            {
                entity = entity.Where(x => x.MaxSalary >= filter.MaxSalary);
            }

            if (!String.IsNullOrEmpty(filter.LocationId))
            {
                entity = entity.Where(s => s.LocationId.Equals(filter.LocationId));
            }
            if (filter.CategoryId > 0)
            {
                entity = entity.Where(x => x.CategoryId == filter.CategoryId);
            }
            if (categoryid > 0)
                entity = entity.Where(x => x.CategoryId == categoryid);

            if (filter.SortBy?.Capacity > 0)
            {
                foreach (var item in filter.SortBy)
                {
                    if (item.IsChecked)
                    {
                        switch (item.Key)
                        {
                            case 1:
                                entity = entity.OrderByDescending(x => x.RatingVotes);
                                break;
                            case 2:
                                entity = entity.OrderBy(x => x.CreatedOn);
                                break;
                            case 3:
                                entity = entity.OrderByDescending(x => x.CreatedOn);
                                break;
                            case 4:
                                entity = entity.OrderBy(x => x.MaxSalary);
                                break;
                        }
                    }
                }
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


      /*  [Route("jobs/new")]
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
        public async Task<IActionResult> Create(CreateJobInputModel input, [FromServices] IBaseService _baseService, [FromServices] INotificationService _notifyService)
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
                input.AllTags = _skillsService.GetAllById<Skills>(input.TagsId, false);
                input.AllLanguages = _langService.GetAll<Language>(input.LanguageId, false);
                input.AllCompanies = _companyService.GetByIdAsync(input.CompanyId).ToAsyncEnumerable();
                input.Worktypes = input.WorkType?.Split(',');

                return View(input);
            }

            input.isArchived = false;
            var result = await _jobsService.Create(input, user);

            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Warning, "Внимание", "Моля изчакайте заявката ви да се прегледа от администратор.", 7000);
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Обявата ви е добавена.", 5000);
                await _notifyService.CreateForAdmins("Обява за работа е в процес на изчакване за преглед", $"", DateTime.Now, NotifyType.Warning, "far fa-clock", user.Id).ConfigureAwait(false);

                return Redirect($"/identity/Jobs/Index");
            }

            return this.View(input);
        }*/
        [AllowAnonymous]
        [Route("jobs/info/{id}")]
        public async Task<IActionResult> Details(int id, [FromServices] IResumeService _resumeService, [FromServices] IFavoritesService _favoritesService)
        {
            var user = await _userManager.GetUserAsync(User);

            var job = await _jobsService.GetByIdAsyncMapped(id);
            if (job is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var company = await _companyService.GetByIdAsync(job.CompanyId);
            if (company == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            job.JobsByCompany = _jobsService.GetAllByEntity(1, true, 6, _favoriteService, user);
            job.CategoryName = await _categoriesService.GetNameById(job.CategoryId);
            job.SkillsMapped = _skillsService.GetAll<SkillsViewModel>(job.TagsId, true);
            job.LanguagesMapped = _langService.GetAll<LanguageViewModel>(job.LanguageId, true);
            job.GalleryImages = company.GalleryImages?.Split(',').ToAsyncEnumerable();
            job.company = company;
            job.GalleryPath = Path.Combine(_GalleryPath, StringHelper.Filter(company.Email));
            job.isInFavourites = user != null ? _favoriteService.isInFavourite(user, PostType.Job, job.Id.ToString()) : false;
            job.ReturnUrl = Url.PageLink();

            if (user is not null)
            {
                string[] items;
                items = job.resumeFilesId?.Split(',');

                var resumes = _resumeService.GetAllAsNoTracking()

                    .Where(x => x.UserId == user.Email)
                    //  .Where(x => !(((IList)items).Contains(x.Id.ToString())))
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Title,
                    });
                ViewData["resumeFiles"] = resumes is null ? null : resumes;
            }

            ///await _jobsService.AddRatingToJobs(job, 1).ConfigureAwait(false);
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

                    switch (T)
                    {
                        case ApproveType.Waiting:
                            await _notifyService.Create("Моля редактирайте вашата обява отново с коректни данни.", "identity/contestant/index", DateTime.Now, NotifyType.Warning, "fas fa-sync-alt", entity.PosterID, null).ConfigureAwait(false);
                        break;
                        case ApproveType.Rejected:
                            await _notifyService.Create("Последно добавената ви обява е отхвърлена.", "identity/contestant/index", DateTime.Now, NotifyType.Danger, "fas fa-ban", entity.PosterID, null).ConfigureAwait(false);
                        break;
                        case ApproveType.Success:
                            await _notifyService.Create("Последно добавената ви обява е одобрена.", "identity/contestant/index", DateTime.Now, NotifyType.Information, "fas fa-check", entity.PosterID, null).ConfigureAwait(false);
                        break;
                    }
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToPage("/Identity/List/Jobs", new { Area = "Identity" });
            }

            return View();
        }

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> ExchangeUser(int id, ApproveType T, string returnUrl, [FromServices] INotificationService _notifyService, [FromServices] IBaseService _baseService)
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

             var result = await _jobsService.UpdateUser(entity, user);
            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Прехвърляне на собственост.", 3000);

                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FavoritesViewModel>> UpdateFavourite(int id)
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
            
           var existed = await _favoriteService.UpdateFavourite(user, PostType.Job, id.ToString()).ConfigureAwait(false);

            var model = new FavoritesViewModel()
            {
               JobCount = await _favoriteService.GetFavouriteByCount(user, PostType.All).ConfigureAwait(false),
               Job = entity,
               isExisted = existed
            };

            return model;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ApplyWithResume(int jobId, JobsViewModel viewModel, string returnUrl, [FromServices] INotificationService _notifyService, [FromServices] IBaseService _baseService, [FromServices] IResumeService _resumeService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            Jobs entity = await _jobsService.GetByIdAsync(jobId);
            if (entity is null)
            {
                return Redirect("/Identity/Account/Errors/NoEntity");
            }

            var result = await _jobsService.AddResumeFile(entity, viewModel.resumeFilesId);
            if (result.Success)
            {
                await _resumeService.Update(int.Parse(viewModel.resumeFilesId), entity.Name);
                await _favoriteService.UpdateFavourite(user, PostType.Job, jobId.ToString()).ConfigureAwait(false);

               string jobIdComplate = ',' + jobId.ToString();
               await _statsService.Update(new StatsInputModel { AppliedJobsId = jobIdComplate }).ConfigureAwait(false);

                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Кандидатствахте по тази обява.", 3000);
            }
            else
            {
                await _notifyService.Create(result.FailureMessage, $"jobs/details/{jobId}", DateTime.Now, NotifyType.Danger, "fas fa-minus-circle", user.Id, null).ConfigureAwait(false);
            }

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> UploadAndApplyResume(int jobId, JobsViewModel entity, string returnUrl, [FromServices] IBaseService _baseService, [FromServices] IResumeService _resumeService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            Jobs job = await _jobsService.GetByIdAsync(jobId);
            if (job is null)
            {
                return Redirect("/Identity/Account/Errors/NoEntity");
            }

            if (entity.File is not null)
            {
                var FileId = await _baseService.UploadFileAsync(entity.File, null, user);

                var lenght = entity.File.FileName.Length > 40 ? entity.File.FileName.Length - 30 : entity.File.FileName.Length;
                int result = await _resumeService.CreateFast(entity.File.FileName.Substring(0, lenght), FileId, job.Id, job.Name, user);

                if (result != -1 && result != -2)
                {

                        var result2 = await _jobsService.AddResumeFile(job, result.ToString());
                        if (result2.Success)
                        {
                            await _favoriteService.UpdateFavourite(user, PostType.Job, jobId.ToString()).ConfigureAwait(false);

                            string jobIdComplate = ',' + jobId.ToString();
                            await _statsService.Update(new StatsInputModel { AppliedJobsId = jobIdComplate }).ConfigureAwait(false);

                        _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Кандидатствахте по тази обява.", 3000);
                    }
                    
                }
                
                else
                {
                    if(result != -1)
                        _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "Вече имате качен файл с това име или сте кандидатствали по тази обява.", 6000);
                    if (result != -2)
                        _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "Лимитът ви за качване на файлове е достигнат, моля прегледайте си ги или изтрийте някой файл.", 6000);
                }
            }

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<ActionResult> ApplyAsGuest(int jobId, ApplyRaportViewModel viewModel, string returnUrl, [FromServices] IBaseService _baseService, [FromServices] IResumeService _resumeService)
        {
            Jobs entity = await _jobsService.GetByIdAsync(jobId);
            if (entity is null)
            {
                return Redirect("/Identity/Account/Errors/NoEntity");
            }


            string fileName = await _baseService.UploadFileAsGuestAsync(viewModel.File, entity.Name, viewModel.Email); //await _jobsService.AddResumeFile(entity, viewModel.Guest.File.FileName);

            var lenght = viewModel.File.FileName.Length > 40 ? viewModel.File.FileName.Length - 30 : viewModel.File.FileName.Length;
            int uploadFileId = await _resumeService.CreateAsGuest(viewModel.File.FileName.Substring(0, lenght), fileName, viewModel.Email, entity.Id, entity.Name);

            if (uploadFileId == -1)
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Неуспешно", "Вече сте кандидатствали по тази обява!", 5000);
            }
            else
            {

                var result = await _jobsService.AddResumeFile(entity, uploadFileId.ToString());
                if (result.Success)
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Кандидатствахте по тази обява.", 3000);
                else
                    _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "вие не успяхте да кандидатсвате. Моля направете си регистрация и опитайте отново!", 5000);
            }
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
    }

}