using HireMe.Entities;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Mapping.Utility;
using HireMe.Services.Interfaces;
using HireMe.ViewModels.Contestants;
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
    public class ContestantsController : BaseController
    {
        private readonly UserManager<User> _userManager;

        private readonly IContestantsService _contestantsService;
        private readonly ICategoriesService _categoriesService;
        private readonly ILanguageService _langService;
        private readonly ISkillsService _skillsService;
        private readonly ILocationService _locationService;

        public ContestantsController(
            UserManager<User> userManager,
            IContestantsService contestantsService,
            ICategoriesService categoriesService,
            ISkillsService skillsService,
            ILocationService locationService,
            ILanguageService langService)
        {
            _userManager = userManager;
            _contestantsService = contestantsService;
            _categoriesService = categoriesService;

            _skillsService = skillsService;
            _locationService = locationService;
            _langService = langService;
        }

        //[HttpGet("/sync-over-async")]
        [HttpGet]
       // [Route("candidates/all")]
        public async Task<IActionResult> Index(int currentPage = 1, string Sort = null)
       {
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
                     CreatedOn = x.CreatedOn,
                     PosterID = x.PosterID,
                     RatingVotes = x.RatingVotes,
                     payRate = x.payRate,
                     SalaryType = x.SalaryType,
                     Promotion = x.Promotion,
                     About = x.About,
                     userSkillsId = x.userSkillsId,
                     LanguagesId = x.LanguagesId
                    });



            if (!String.IsNullOrEmpty(viewModel.LanguagesId))
            {
                string[] langs = viewModel.LanguagesId?.Split(", ");
                entity = entity.Where(x => ((IList)langs).Contains(x.LanguagesId));
            }
            if (!String.IsNullOrEmpty(viewModel.WorkType))
            {
                string[] type = viewModel.WorkType?.Split("%2C");
                entity = entity.Where(x => ((IList)type).Contains(x.WorkType));
            }
            if (!String.IsNullOrEmpty(viewModel.LocationId))
            {
                entity = entity.Where(s => s.LocationId.Equals(viewModel.LocationId));
            }
            if (viewModel.CategoryId > 0)
            {
                entity = entity.Where(x => x.CategoryId == viewModel.CategoryId);
            }

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
                    entity = entity.OrderBy(x => x.payRate);
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
           
                viewModel.Result = result.To<ContestantViewModel>().AsAsyncEnumerable();
            }
            else viewModel.Result = null;
            

            return View(viewModel);
        }


        [Route("candidates/new")]
        [Authorize(Roles = "Admin,Moderator,User,Contestant")]
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
                return Redirect("/Identity/Account/Manage/EditProfile");
            }
            CreateContestantInputModel viewModel = new CreateContestantInputModel();
            viewModel.AllLocations = _locationService.GetAllSelectList();
            viewModel.AllCategories = _categoriesService.GetAllSelectList();

            return View(viewModel);
        }

        [HttpPost]
        [Route("candidates/new")]
        [Authorize(Roles = "Admin,Moderator,User,Contestant")]
        public async Task<IActionResult> Create(CreateContestantInputModel input, [FromServices] IBaseService _baseService)
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


            if (input.FormFile != null)
                input.ResumeFileId = await _baseService.UploadFileAsync(input.FormFile, null, user);

            input.isArchived = false;

            var result = await this._contestantsService.Create(input, user);

            if (result.Success)
            {
                if (user.Role == Roles.User)
                {
                    await _userManager.AddToRoleAsync(user, "Contestant");
                    await _userManager.RemoveFromRoleAsync(user, "User");
                    user.Role = Roles.Contestant;
                    await _userManager.UpdateAsync(user);
                }
                _baseService.ToastNotify(ToastMessageState.Warning, "Внимание", "Моля изчакайте заявката ви да се прегледа от администратор.", 7000);
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Кандидатурата ви е добавена.", 5000);
                return Redirect($"/identity/contestant/index");
            }

            return View(input);
        }


        [AllowAnonymous]
        [Route("candidates/info")]
        public async Task<IActionResult> Details(int id)
        {
            var contestant = await _contestantsService.GetByIdAsyncMapped(id);
            if (contestant == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            contestant.SkillsMapped = _skillsService.GetAll<SkillsViewModel>(contestant.userSkillsId, true);
            contestant.Languages = _langService.GetAll(contestant.LanguagesId);

            return this.View(contestant);
        }

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Approve(int id, ApproveType T, string returnUrl, [FromServices] IBaseService _baseService, [FromServices] INotificationService _notifyService)
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

            var result = await _baseService.Approve(id, PostType.Contestant, T);

            if (result.Success)
            {
                var PosterId = await _userManager.FindByIdAsync(entity.PosterID);

                if (!(PosterId is null))
                {

                    switch (T)
                    {
                        case ApproveType.Waiting:
                            await _notifyService.Create("Моля редактирайте вашата кандидатура отново с коректни данни.", "identity/contestant/index", DateTime.Now, NotifyType.Warning, "fas fa-sync-alt", PosterId);
                            break;
                        case ApproveType.Rejected:
                            await _notifyService.Create("Последно добавената ви кандидатура е отхвърлена.", "identity/contestant/index", DateTime.Now, NotifyType.Danger, "fas fa-ban", PosterId);
                            break;
                        case ApproveType.Success:
                            await _notifyService.Create("Последно добавената ви кандидатура е одобрена.", "identity/contestant/index", DateTime.Now, NotifyType.Information, "fas fa-check", PosterId);
                            break;
                    }

                }
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
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

            if(!(await _favoriteService.isInFavourite(user, PostType.Contestant, id)))
            await _favoriteService.AddToFavourite(user, PostType.Contestant, id.ToString());
            else
            await _favoriteService.RemoveFromFavourite(user, PostType.Contestant, id.ToString());

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }


        /*private static List<int> ToWordsList(string words)
        {
            return string.IsNullOrWhiteSpace(words) ? new List<int>() : words.Split(",").ToList().ConvertAll(int.Parse);
        }*/
    }
}
