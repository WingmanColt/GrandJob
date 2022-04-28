using HireMe.Core.Helpers;
using HireMe.Entities;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Mapping.Utility;
using HireMe.Services.Interfaces;
using HireMe.Utility;
using HireMe.ViewModels.Contestants;
using HireMe.ViewModels.Favorites;
using HireMe.ViewModels.Language;
using HireMe.ViewModels.Skills;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    public class ContestantsController : BaseController
    {
        private readonly UserManager<User> _userManager;

        private readonly IConfiguration _config;
        private readonly IContestantsService _contestantsService;
        private readonly IContestantDetailsService _contestantDetailsService;
        private readonly ICategoriesService _categoriesService;
        private readonly ILanguageService _langService;
        private readonly ISkillsService _skillsService;
        private readonly ILocationService _locationService;

        private readonly IFavoritesService _favoriteService;

        public ContestantsController(
            IConfiguration config,
            UserManager<User> userManager,
            IContestantsService contestantsService,
            IContestantDetailsService contestantDetailsService,
            ICategoriesService categoriesService,
            ISkillsService skillsService,
            ILocationService locationService,
            ILanguageService langService,
            IFavoritesService favoriteService)
        {
            _config = config;
            _userManager = userManager;

            _contestantsService = contestantsService;
            _contestantDetailsService = contestantDetailsService;
            _categoriesService = categoriesService;

            _skillsService = skillsService;
            _locationService = locationService;
            _langService = langService;
            _favoriteService = favoriteService;
        }

        //[HttpGet("/sync-over-async")]
        [HttpGet]
        [Route("candidates/all")]
        public async Task<IActionResult> Index(int currentPage = 1)
       {
            var user = await _userManager.GetUserAsync(User);

            var viewModel = new ContestantViewModel
            {
                AllLocations = _locationService.GetAllSelectList(),
                AllCategories = _categoriesService.GetAllSelectList()
            };
            viewModel.Skills = _skillsService.GetAll<Skills>(viewModel.SkillsId, false);
            //viewModel.ReturnUrl = Url.Page(null, pageHandler: null, new { currentPage = currentPage }, protocol: Request.Scheme); 

            var entity = _contestantsService.GetAllAsNoTracking()
                    .Where(x => (x.isApproved == ApproveType.Success) && !x.isArchived && (x.profileVisiblity == 0))
                    .Select(x => new Contestant
                    {
                     Id = x.Id,
                     FullName = x.FullName,
                     LocationId = x.LocationId,
                     SalaryType = x.SalaryType,
                     userSkillsId = x.userSkillsId,
                     Logo = x.Logo,
                     payRate = x.payRate,
                     Speciality = x.Speciality,
                     Promotion = x.Promotion,
                     isInFavourites = _favoriteService.isInFavourite(user, PostType.Contestant, x.Id.ToString())
                    });


            if (await entity.AnyAsync()) // prevent 'SqlException: The offset specified in a OFFSET clause may not be negative.'
            {
                int count = await entity.AsQueryable().AsNoTracking().CountAsync().ConfigureAwait(false);
                viewModel.Pager = new Pager(count, currentPage);

                var result = entity
                .Skip((viewModel.Pager.CurrentPage - 1) * viewModel.Pager.PageSize)
                .Take(viewModel.Pager.PageSize);
           
                viewModel.Result = result.To<ContestantViewModel>().AsAsyncEnumerable();
            }
            else viewModel.Result = null;
            

            return View(viewModel);
        }

        [HttpPost]
        [Route("candidates/all")]
        public async Task<IActionResult> Index(Filter filter, int currentPage = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new ContestantViewModel
            {
                AllLocations = _locationService.GetAllSelectList(),
                AllCategories = _categoriesService.GetAllSelectList()
            };
            viewModel.Skills = _skillsService.GetAll<Skills>(viewModel.SkillsId, false);

            var entity = _contestantsService.GetAllAsNoTracking()
                    .Where(x => (x.isApproved == ApproveType.Success) && !x.isArchived && (x.profileVisiblity == 0))
                    .Select(x => new Contestant
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        LocationId = x.LocationId,
                        CreatedOn = x.CreatedOn,
                        PosterID = x.PosterID,
                        RatingVotes = x.RatingVotes,
                        payRate = x.payRate,
                        SalaryType = x.SalaryType,
                        userSkillsId = x.userSkillsId,
                        LanguagesId = x.LanguagesId,
                        CategoryId = x.CategoryId,
                        Logo = x.Logo,
                        Speciality = x.Speciality,
                        Promotion = x.Promotion,
                        isInFavourites = _favoriteService.isInFavourite(user, PostType.Contestant, x.Id.ToString())
                    });


            if (!String.IsNullOrEmpty(filter.SearchString))
            {
                string[] skills = filter.SearchString?.Split("%2C");
                entity = entity.Where(x => ((IList)skills).Contains(x.userSkillsId));
            }
            if (!String.IsNullOrEmpty(filter.LanguageId))
            {
                string[] langs = filter.LanguageId?.Split(", ");
                entity = entity.Where(x => ((IList)langs).Contains(x.LanguagesId));
            }
            if (!String.IsNullOrEmpty(filter.LocationId))
            {
                entity = entity.Where(s => s.LocationId.Equals(filter.LocationId));
            }
            if (filter.CategoryId > 0)
            {
                entity = entity.Where(x => x.CategoryId == filter.CategoryId);
            }


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
                                entity = entity.OrderBy(x => x.payRate);
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

                viewModel.Result = result.To<ContestantViewModel>().AsAsyncEnumerable();
            }
            else viewModel.Result = null;


            return View(viewModel);
        }


        [AllowAnonymous]
        [Route("candidates/info/{id}")]
        public async Task<ActionResult> Details(int id, [FromServices] IResumeService _resumeService, [FromServices] ICategoriesService _categoryService)
        {
            var contestant = await _contestantsService.GetByIdAsyncMapped(id);
            if (contestant == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var user = await _userManager.FindByIdAsync(contestant.PosterID);
            if (user == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            contestant.ContestantDetails_Educations = _contestantDetailsService.GetEducationsByUser(contestant.PosterID);
            contestant.ContestantDetails_Works = _contestantDetailsService.GetWorksByUser(contestant.PosterID);
            contestant.ContestantDetails_Awards = _contestantDetailsService.GetAwardsByUser(contestant.PosterID);

            // Resume
            string SiteResumeUrl = _config.GetSection("MySettings").GetSection("SiteResumeUrl").Value;
            contestant.resumeFullPath = SiteResumeUrl + StringHelper.Filter(user.Email) + contestant.ResumeFileId;

            // Image
            string _ImagePath = _config.GetSection("MySettings").GetSection("UserPicturePath").Value;
            contestant.imageFullPath = (contestant.Logo is not null && contestant.Logo.Contains("https://")) ? contestant.Logo : (_ImagePath + contestant.Logo);

            contestant.SkillsMapped = _skillsService.GetAll<SkillsViewModel>(contestant.userSkillsId, true);
            contestant.LanguagesMapped = _langService.GetAll<LanguageViewModel>(contestant.LanguagesId, true);

            contestant.CategoryName = await _categoryService.GetNameById(contestant.CategoryId);

            return View(contestant);
        }



        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Approve(int id, ApproveType T, string returnUrl, [FromServices] IBaseService _baseService, [FromServices] INotificationService _notifyService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var contestant = await _contestantsService.GetByIdAsync(id);
            if (contestant == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var result = await _baseService.Approve(id, PostType.Contestant, T);

            if (result.Success)
            {
                    switch (T)
                    {
                        case ApproveType.Waiting:
                            await _notifyService.Create("Моля редактирайте вашата кандидатура отново с коректни данни.", "identity/contestant/index", DateTime.Now, NotifyType.Warning, "fas fa-sync-alt", contestant.PosterID, user.Id).ConfigureAwait(false);
                            break;
                        case ApproveType.Rejected:
                            await _notifyService.Create("Последно добавената ви кандидатура е отхвърлена.", "identity/contestant/index", DateTime.Now, NotifyType.Danger, "fas fa-ban", contestant.PosterID, user.Id).ConfigureAwait(false);
                            break;
                        case ApproveType.Success:
                            await _notifyService.Create("Последно добавената ви кандидатура е одобрена.", "identity/contestant/index", DateTime.Now, NotifyType.Information, "fas fa-check", contestant.PosterID, user.Id).ConfigureAwait(false);
                            break;
                    }
                
            }
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToPage("/Identity/List/Contestants", new { Area = "Identity" });
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

            var entity = await _contestantsService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var existed = await _favoriteService.UpdateFavourite(user, PostType.Contestant, id.ToString()).ConfigureAwait(false);

            var model = new FavoritesViewModel()
            {
                JobCount = await _favoriteService.GetFavouriteByCount(user, PostType.All).ConfigureAwait(false),
                Contestant = entity,
                isExisted = existed
            };

            return model;
        }
    }
}
