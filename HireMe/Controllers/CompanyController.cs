using HireMe.Core.Helpers;
using HireMe.Entities;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Mapping.Utility;
using HireMe.Services.Interfaces;
using HireMe.StoredProcedures.Enums;
using HireMe.StoredProcedures.Interfaces;
using HireMe.ViewModels.Company;
using HireMe.ViewModels.Favorites;
using HireMe.ViewModels.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly ICompanyService _companyService;
        private readonly ILocationService _locationService;
        private readonly IBaseService _baseService;
        private readonly IFavoritesService _favoriteService;

        private readonly string _GalleryPath;

        public CompanyController(
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IBaseService baseService,
            ICompanyService companyService,
            ILocationService locationService,
            IFavoritesService favoriteService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _baseService = baseService;
            _companyService = companyService;
            _locationService = locationService;
            _favoriteService = favoriteService;

            _GalleryPath = config.GetValue<string>("StoredGalleryPath");
        }


        [HttpPost]
        public IActionResult Upload()
        {
            var data = Request.Form; //This is 
            return View();
        }

        [HttpGet]
        [Route("companies/all")]
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new CompanyViewModel();

            var entity = _companyService.GetAllAsNoTracking()
                 .Where(x => (x.isApproved == ApproveType.Success) && x.isAuthentic_EIK)
                 .Select(x => new Company
                {
                    Id = x.Id,
                    Title = x.Title,
                    Logo = x.Logo,
                    LocationId = x.LocationId,
                    RatingVotes = x.RatingVotes,
                    Promotion = x.Promotion, 
                    Rating = x.Rating,
                    Date = x.Date,
                    isInFavourites = _favoriteService.isInFavourite(user, PostType.Company, x.Id.ToString())
                 });


            if (await entity.AnyAsync()) // prevent 'SqlException: The offset specified in a OFFSET clause may not be negative.'
            {
                int count = await entity.AsQueryable().AsNoTracking().CountAsync().ConfigureAwait(false);
                viewModel.Pager = new Pager(count, currentPage);

                var result = entity
                .Skip((viewModel.Pager.CurrentPage - 1) * viewModel.Pager.PageSize)
                .Take(viewModel.Pager.PageSize);

                viewModel.Result = result.To<CompanyViewModel>().AsAsyncEnumerable();
            }
            else viewModel.Result = null;

            return View(viewModel);
        }


        [HttpPost]
        [Route("companies/all")]
        public async Task<IActionResult> Index(CompanyViewModel viewModel, Filter filter, int currentPage = 1)
        {
           // var viewModel = new CompanyViewModel();
           /* {
                AllLocations = _locationService.GetAllSelectList()
            };*/

            var entity = _companyService.GetAllAsNoTracking()
                 .Where(x => (x.isApproved == ApproveType.Success) && x.isAuthentic_EIK)
                /*.Select(x => new Company
                {
                    Id = x.Id,
                    Title = x.Title,
                    Logo = x.Logo,
                    LocationId = x.LocationId,
                    RatingVotes = x.RatingVotes,
                    Promotion = x.Promotion,
                    Adress = x.Adress,
                    Email = x.Email,
                    About = x.About,    
                    GalleryImages = x.GalleryImages,    
                    PhoneNumber = x.PhoneNumber,
                    Website = x.Website,
                    Linkdin = x.Linkdin,
                    Twitter = x.Twitter,
                    Facebook = x.Facebook,
                    Rating = x.Rating
                })*/;

            if (!String.IsNullOrEmpty(filter.Name))
            {
                entity = entity.Where(x => x.Title.Contains(filter.Name));
            }


            if (!String.IsNullOrEmpty(filter.LocationId))
            {
                entity = entity.Where(s => s.LocationId.Equals(filter.LocationId));
            }


            /// !!!! fix after add category to firms !!!!! ///
            if (filter.CategoryId > 0)
            {
                entity = entity.Where(s => s.LocationId.Equals(filter.CategoryId));
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
                                entity = entity.OrderBy(x => x.Date);
                                break;
                            case 3:
                                entity = entity.OrderByDescending(x => x.Date);
                                break;
                            case 4:
                                entity = entity.OrderBy(x => x.Date);
                                break;
                        }
                    }
                }
            }
            if (await entity.AnyAsync()) // prevent 'SqlException: The offset specified in a OFFSET clause may not be negative.'
            {
                int count = await entity.AsQueryable().AsNoTracking().CountAsync().ConfigureAwait(true);
                viewModel.Pager = new Pager(count, currentPage);

                var result = entity
                .Skip((viewModel.Pager.CurrentPage - 1) * viewModel.Pager.PageSize)
                .Take(viewModel.Pager.PageSize);

                viewModel.Result = result.To<CompanyViewModel>().AsAsyncEnumerable();
            }
            else viewModel.Result = null;

            return View(viewModel);
        }


        [AllowAnonymous]
        [Route("companies/info/{id}")]
        public async Task<IActionResult> Details(int id, [FromServices] IspJobService _spJobService, [FromServices] IJobsService _jobsService, [FromServices] ICategoriesService _categoryService, [FromServices] IFavoritesService _favouriteService)
        {
            var user = await _userManager.GetUserAsync(User);

            var company = await _companyService.GetByIdAsyncMapped(id);
            if (company is null)
            {
                return RedirectToAction("Index", "Company");
            }

            company.JobsByCompany = await _spJobService.GetAll<JobsViewModel>(new { CompanyId = company?.Id, CurrentUser = user?.Id }, JobGetActionEnum.GetAllBy, false, null);
            
            company.JobsCount =  await _spJobService.GetAllCountBy(new { CompanyId = company?.Id }).ConfigureAwait(false);
            company.GalleryImagesList = company.GalleryImages?.Split(',').ToAsyncEnumerable();
            company.GalleryPath = Path.Combine(_GalleryPath, StringHelper.Filter(company.Email));
            company.CategoryName = await _categoryService.GetNameById(company.CategoryId);
            
            return this.View(company);
        }

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Approve(int id, ApproveType T, string returnUrl, [FromServices] INotificationService _notifyService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var company = await _companyService.GetByIdAsync(id);
            if (company == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var result = await this._baseService.Approve(id, PostType.Company, T);

            if (result.Success)
            {                 
                    switch (T)
                    {
                        case ApproveType.Waiting:
                            await _notifyService.Create("Моля редактирайте вашата фирма отново с коректни данни.", "identity/list/companies", DateTime.Now, NotifyType.Warning, null, company.PosterId, user.Id).ConfigureAwait(false);
                        break;
                        case ApproveType.Rejected:
                            await _notifyService.Create("Последно добавената ви фирма е отхвърлена.", "identity/list/companies", DateTime.Now, NotifyType.Danger, null, company.PosterId, user.Id).ConfigureAwait(false);
                        break;
                        case ApproveType.Success:
                            await _notifyService.Create("Последно добавената ви фирма е одобрена.", "identity/list/companies", DateTime.Now, NotifyType.Information, null, company.PosterId, user.Id).ConfigureAwait(false);
                        break;
                    }   
            }

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToPage("/Identity/List/Companies", new { Area = "Identity" });
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

            var entity = await _companyService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var existed = await _favoriteService.UpdateFavourite(user, PostType.Company, id.ToString()).ConfigureAwait(false);

            var model = new FavoritesViewModel()
            {
                JobCount = await _favoriteService.GetFavouriteByCount(user, PostType.All).ConfigureAwait(false),
                Company = entity,
                isExisted = existed
            };

            return model;
        }
        [HttpPost]
        [Produces("application/json")]
        public JsonResult isEIKValid(string eik)
        {
            bool isValid = EikValidator.checkEIK(eik);
            return Json(isValid);
        }

    }
}