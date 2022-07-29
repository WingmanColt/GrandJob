using HireMe.Core.Helpers;
using HireMe.Entities;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using HireMe.StoredProcedures.Enums;
using HireMe.StoredProcedures.Interfaces;
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
        private readonly IspJobService _spJobService;

        private readonly IPromotionService _promotionService;

        private readonly ISkillsService _skillsService;
        private readonly ILanguageService _langService;

        private readonly IStatisticsService _statsService;
        private readonly IFavoritesService _favoriteService;
        private readonly IFilesService _filesService;

        private readonly IBaseService _baseService;
        private readonly IResumeService _resumeService;
        private readonly INotificationService _notifyService;

        private readonly string _GalleryPath;

        public JobsController(
            IConfiguration config,
            UserManager<User> userManager,
            IBaseService baseService,
            IJobsService jobsService,
            IspJobService spJobService,
            ICompanyService companyService,
            IPromotionService promotionService,
            ICategoriesService categoriesService,
            ILocationService locationService,
            ISkillsService skillsService,
            ILanguageService langService,
            IStatisticsService statsService,
            IFilesService filesService,
            IResumeService resumeService,
            INotificationService notifyService,
            IFavoritesService favoriteService)
        {
            _userManager = userManager;
            _baseService = baseService;

            _jobsService = jobsService;
            _spJobService = spJobService;
            _companyService = companyService;
            _promotionService = promotionService;

            _locationService = locationService;
            _skillsService = skillsService;
            _langService = langService;

            _categoriesService = categoriesService;
            _statsService = statsService;
            _favoriteService = favoriteService;
            _filesService = filesService;
            _resumeService = resumeService;
            _notifyService = notifyService;

            _GalleryPath = config.GetValue<string>("StoredGalleryPath");
        }

        [HttpGet]
        [Route("jobs/all")]
        public async Task<IActionResult> Index(int currentPage = 1, string SearchString = null, string LocationId = null)
        {
            // var filter = new Filter();
            var viewModel = new JobsViewModel
            {
                AllLocations = _locationService.GetAllSelectList(),
                AllCategories = _categoriesService.GetAllSelectList()            
            };

            viewModel.Skills = _skillsService.GetAll<Skills>(viewModel.TagsId, false);

            var entity = await _spJobService.GetAll<JobsViewModel>(new { Name = SearchString, LocationId = LocationId }, JobGetActionEnum.GetAllFiltering, false, null);
            
            if (await entity.AnyAsync()) // prevent 'SqlException: The offset specified in a OFFSET clause may not be negative.'
            {
                int count = await entity.CountAsync().ConfigureAwait(false);
                viewModel.Pager = new Pager(count, currentPage);

                viewModel.Result = entity
                .Skip((viewModel.Pager.CurrentPage - 1) * viewModel.Pager.PageSize)
                .Take(viewModel.Pager.PageSize);
            }
            else viewModel.Result = null;

            return View(viewModel);
        }

        [HttpPost]
        [Route("jobs/all")]
        public async Task<IActionResult> Index(Filter filter, int currentPage, int categoryid)
        {
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


            var entity = await _spJobService.GetAll<JobsViewModel>(filter, JobGetActionEnum.GetAllFiltering, true, "NotMapped");

            /*var entity = _jobsService.GetAllAsNoTracking()
                .Where(x => (x.isApproved == ApproveType.Success) && !x.isArchived)
                .OrderByDescending(x => x.PremiumPackage)
                .OrderByDescending(x => x.Rating)
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
                    PremiumPackage = x.PremiumPackage,
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

            */

            if (await entity.AnyAsync()) // prevent 'SqlException: The offset specified in a OFFSET clause may not be negative.'
            {
                int count = await entity.CountAsync().ConfigureAwait(false);
                viewModel.Pager = new Pager(count, currentPage);

                viewModel.Result = entity
                .Skip((viewModel.Pager.CurrentPage - 1) * viewModel.Pager.PageSize)
                .Take(viewModel.Pager.PageSize);


                if (filter.SortBy?.Capacity > 0)
                {
                    foreach (var item in filter.SortBy)
                    {
                        if (item.IsChecked)
                        {
                            switch (item.Key)
                            {
                                case 1:
                                    viewModel.Result = viewModel.Result.OrderByDescending(x => x.RatingVotes);
                                    break;
                                case 2:
                                    viewModel.Result = viewModel.Result.OrderBy(x => x.CreatedOn);
                                    break;
                                case 3:
                                    viewModel.Result = viewModel.Result.OrderByDescending(x => x.CreatedOn);
                                    break;
                                case 4:
                                    viewModel.Result = viewModel.Result.OrderBy(x => x.MaxSalary);
                                    break;
                            }
                        }
                    }
                }
                //viewModel.Result = result.To<JobsViewModel>().AsAsyncEnumerable();
            }
            else viewModel.Result = null;

            return View(viewModel);
        }


        [AllowAnonymous]
        [Route("jobs/info/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var job = await _spJobService.GetByIdAsync<JobsViewModel>(id);
            if (job is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var company = await _companyService.GetByIdAsync(job.CompanyId);
            if (company == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            job.JobsByCompany = await _spJobService.GetAll<JobsViewModel>(new { CompanyId = company?.Id }, JobGetActionEnum.GetAllBy, false, null);
            job.CategoryName = await _categoriesService.GetNameById(job.CategoryId);
            job.SkillsMapped = _skillsService.GetAll<SkillsViewModel>(job.TagsId, true);
            job.LanguagesMapped = _langService.GetAll<LanguageViewModel>(job.LanguageId, true);
            job.GalleryImages = company.GalleryImages?.Split(',').ToAsyncEnumerable();
            job.company = company;
            job.GalleryPath = Path.Combine(_GalleryPath, StringHelper.Filter(company.Email));
           // job.isInFavourites = user != null ? _favoriteService.isInFavourite(user, PostType.Job, job.Id.ToString()) : false;
            job.ReturnUrl = Url.PageLink();
            job.Views++;

            if (user is not null)
            {
                var files = GetFilesByEmail(user.Email);
                ViewData["myFiles"] = files is null ? null : files;
            } 

            ///await _jobsService.AddRatingToJobs(job, 1).ConfigureAwait(false);
            return this.View(job);
        }


        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Approve(int id, ApproveType T, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var entity = await _spJobService.GetByIdAsync<Jobs>(id);
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
                            await _notifyService.Create("Моля редактирайте вашата обява отново с коректни данни.", "identity/list/jobs", DateTime.Now, NotifyType.Warning, null, entity.PosterID, null).ConfigureAwait(false);
                        break;
                        case ApproveType.Rejected:
                            await _notifyService.Create("Последно добавената ви обява е отхвърлена.", "identity/list/jobs", DateTime.Now, NotifyType.Danger, null, entity.PosterID, null).ConfigureAwait(false);
                        break;
                        case ApproveType.Success:
                        await _categoriesService.Update(entity.CategoryId, true, CategoriesEnum.Increment).ConfigureAwait(false);
                        await _notifyService.Create("Последно добавената ви обява е одобрена.", "identity/list/jobs", DateTime.Now, NotifyType.Information, null, entity.PosterID, null).ConfigureAwait(false);
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
        public async Task<ActionResult> ExchangeUser(int id, ApproveType T, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var entity = await _spJobService.GetByIdAsync<Jobs>(id);
            if (entity == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

             var result = await _spJobService.CRUD(new { Id = entity.Id, PosterId = user.Id }, JobCrudActionEnum.UpdateUser, false, null, null);
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
        public async Task<ActionResult> RefreshPost(int id, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var entity = await _spJobService.GetByIdAsync<Jobs>(id);
            if (entity == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            Promotion existEntity = await _promotionService.GetPromotion(PostType.Job, entity.Id);
            var result = await _spJobService.CRUD(entity, JobCrudActionEnum.RefreshDate, false, null, null);
            if (result.Success)
            {
                if (existEntity is not null)
                {
                    existEntity.RefreshCount -= 1;
                    await _promotionService.Update(existEntity, user);

                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Обновихте обявата си.", 3000);
                }
                else _baseService.ToastNotify(ToastMessageState.Error, "Несупешна операция", "Трябва да сте промотирали обявата си първо.", 6000);
            }
            else _baseService.ToastNotify(ToastMessageState.Error, "Несупешна операция", "Нямате в наличност обновления за тази обява.", 6000);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
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

            var entity = await _spJobService.GetByIdAsync<Jobs>(id);
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
        public async Task<ActionResult> ApplyWithMyFiles(int jobId, JobsViewModel viewModel, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            Jobs entity = await _spJobService.GetByIdAsync<Jobs>(jobId);
            if (entity is null)
            {
                return Redirect("/Identity/Account/Errors/NoEntity");
            }


            var fileEntity = await _filesService.GetByIdAsync(int.Parse(viewModel.resumeFilesId));
            if (fileEntity is null)
            {
                return Redirect("/Identity/Account/Errors/NoEntity");
            }


            //var result = await _jobsService.AddResumeFile(entity, viewModel.resumeFilesId);
            //if (result.Success)
            //{
                var resumeIsExists = await _resumeService.IsResumeExists(user.Email, entity.Id, fileEntity.Title).ConfigureAwait(true);
                if (!resumeIsExists)
                {
                    await _resumeService.Create(fileEntity.Title, entity.Name, fileEntity.FileId, jobId, user);
                    await _favoriteService.UpdateFavourite(user, PostType.Job, jobId.ToString()).ConfigureAwait(false);

                 //   string jobIdComplate = ',' + jobId.ToString();
                  //  await _statsService.Update(new StatsInputModel { AppliedJobsId = jobIdComplate }).ConfigureAwait(false);

                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Кандидатствахте по тази обява.", 3000);
                    await _notifyService.Create($"Имате нов кандидат за вашата обява - {entity.Name}.", "identity/account/manage", DateTime.Now, NotifyType.Information, null, entity.PosterID, null).ConfigureAwait(false);

                    await _baseService.CloneFileAsync(fileEntity.FileId, StringHelper.FilterTrimSplit(entity.Name), user).ConfigureAwait(false);
                }
                else _baseService.ToastNotify(ToastMessageState.Warning, "Забележка", "Вече сте кандидатствали по тази обява.", 3000);
            /*}
            else
            {
                await _notifyService.Create(result.FailureMessage, $"jobs/info/{jobId}", DateTime.Now, NotifyType.Danger, "fas fa-minus-circle", user.Id, null).ConfigureAwait(false);
            }*/

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> UploadAndApplyMyFile(int jobId, JobsViewModel entity, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            Jobs job = await _spJobService.GetByIdAsync<Jobs>(jobId);
            if (job is null)
            {
                return Redirect("/Identity/Account/Errors/NoEntity");
            }

            if (entity.File is not null)
            {
                var FileId = await _baseService.UploadFileAsync(entity.File, null, job.Name, FileType.AppliedCV, user);

                var lenght = entity.File.FileName.Length > 40 ? entity.File.FileName.Length - 30 : entity.File.FileName.Length;
                int createFastResume = await _resumeService.CreateFast(entity.File.FileName.Substring(0, lenght), FileId, job.Id, job.Name, user);

                if (createFastResume != -1 && createFastResume != -2)
                {
                       // var createResumeResult = await _jobsService.AddResumeFile(job, createFastResume.ToString());
                        //if (createResumeResult.Success)
                        //{ 

                        await _filesService.Create(entity.File.FileName.Substring(0, lenght), job.Name, FileId, user);

                        await _favoriteService.UpdateFavourite(user, PostType.Job, jobId.ToString()).ConfigureAwait(false);

                        string jobIdComplate = ',' + jobId.ToString();
                        //await _statsService.Update(new StatsInputModel { AppliedJobsId = jobIdComplate }).ConfigureAwait(false);

                        _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Кандидатствахте по тази обява.", 3000);
                        await _notifyService.Create($"Имате нов кандидат за вашата обява - {entity.Name}.", "identity/account/manage", DateTime.Now, NotifyType.Information, null, entity.PosterID, null).ConfigureAwait(false);

                    //}

                }
                
                else
                {
                    if(createFastResume != -1)
                        _baseService.ToastNotify(ToastMessageState.Error, "Несупешна операция", "Вече имате качен файл с това име или сте кандидатствали по тази обява.", 6000);
                    if (createFastResume != -2)
                        _baseService.ToastNotify(ToastMessageState.Error, "Несупешна операция", "Лимитът ви за качване на файлове е достигнат, моля прегледайте си ги или изтрийте някой файл.", 6000);
                }
            }

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public async Task<ActionResult> ApplyAsGuest(int jobId, ApplyRaportViewModel viewModel, string returnUrl)
        {
            Jobs entity = await _spJobService.GetByIdAsync<Jobs>(jobId);
            if (entity is null)
            {
                return Redirect("/Identity/Account/Errors/NoEntity");
            }


            string fileName = await _baseService.UploadFileAsync(viewModel.File, FileType.GuestsCV, entity.Name, viewModel.Email); 

            var lenght = viewModel.File.FileName.Length > 40 ? viewModel.File.FileName.Length - 30 : viewModel.File.FileName.Length;
            int uploadFileId = await _resumeService.CreateAsGuest(viewModel.File.FileName.Substring(0, lenght), fileName, viewModel.Email, entity.Id, entity.Name);

            if (uploadFileId == -1)
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Несупешна операция", "Вече сте кандидатствали по тази обява!", 5000);
            }
            else
            {

               // var result = await _jobsService.AddResumeFile(entity, uploadFileId.ToString());
                //if (result.Success)
                //{
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Кандидатствахте по тази обява.", 3000);
                    await _notifyService.Create($"Имате нов кандидат за вашата обява - {entity.Name}.", "identity/account/manage", DateTime.Now, NotifyType.Information, null, entity.PosterID, null).ConfigureAwait(false);

               // }
              //  else
               //     _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "вие не успяхте да кандидатсвате. Моля направете си регистрация и опитайте отново!", 5000);
            }
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        public IQueryable<SelectListItem> GetFilesByEmail(string Email)
        {
            return _filesService.GetAllAsNoTracking().Where(x => x.UserId == Email)
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Title });
        }
    }

}