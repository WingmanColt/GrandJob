
namespace HireMe.Areas.Identity.Pages
{
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using HireMe.StoredProcedures.Enums;
    using HireMe.StoredProcedures.Interfaces;
    using HireMe.ViewModels.PartialsVW;
    using HireMe.Web.Areas.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    public partial class ListModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;

        private readonly IBaseService _baseService;
        private readonly IspJobService _spJobsService;
        private readonly IJobsService _JobsService;
        private readonly ICompanyService _companyService;
        private readonly IContestantsService _contestantService;
        private readonly IPromotionService _promotionService;

        private readonly string _CompanyPathShow;
        private readonly string _UserPathShow;

        public ListModel(
            IConfiguration config,
            UserManager<User> userManager,
            IBaseService baseService,
            IspJobService spJobsService,
            IJobsService JobsService,
            ICompanyService companyService,
            IContestantsService contestantService,
            IPromotionService promotionService)
        {
            _config = config;
            _userManager = userManager;
            _baseService = baseService;
            _spJobsService = spJobsService;
            _JobsService = JobsService;
            _companyService = companyService;
            _contestantService = contestantService;

            _CompanyPathShow = config.GetValue<string>("MySettings:CompanyImageUrl");
            _UserPathShow = config.GetValue<string>("MySettings:UserPicturePath");
            _promotionService = promotionService;
        }

        // [BindProperty]
        public Pager Pager { get; set; }
        public User UserEntity { get; set; }

        [BindProperty]
        public DeleteViewModel Model { get; set; }

        public IAsyncEnumerable<JobsList> JobsList { get; set; }
        public IAsyncEnumerable<Company> CompaniesList { get; set; }
        public IAsyncEnumerable<Entities.Models.Contestant> ContestantsList { get; set; }

        public string Sort { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Моля въведете вашата парола.")]
            [Display(Name = "Парола:")]
            [DataType(DataType.Password, ErrorMessage = "Грешна парола.")]
            public string Password { get; set; }

            public string Handler { get; set; }
            public int CurrentPage { get; set; }
        }
        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;

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
            Pager = new Pager(0, 1);
            return Page();
        }
       // [AuthorizeRoles(Roles.Employer)]
        public async Task<IActionResult> OnGetJobsAsync(int currentPage = 1, string Sort = null)
        {
            TempData["ReturnUrl"] = Url.Page("List", "Jobs", new { currentPage = currentPage, Sort = Sort, area = "Identity" });

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

            var Tag = Roles.Employer | Roles.Recruiter;
            var has = user.Role.HasFlag(Tag);
            var all = await _spJobsService.GetAll<JobsList>(new { PosterId = user.Role.HasFlag(Tag) ? user?.Id : null }, JobGetActionEnum.GetAllForDashboard, false, null);

            /*if (user.Role.Equals(Roles.Employer) || user.Role.Equals(Roles.Recruiter))
            {
                all = all.Where(x => x.PosterID == user.Id);
            }

            all = all.Select(x => new HireMe.Entities.Models.Jobs
                 {
                                   Id = x.Id,
                                   Name = x.Name,
                                   CompanyLogo = _CompanyPathShow + x.CompanyLogo,
                                   CreatedOn = x.CreatedOn,
                                   ExpiredOn = x.ExpiredOn,
                                   isArchived = x.isArchived,
                                   isApproved = x.isApproved,
                                   PremiumPackage = x.PremiumPackage

                });*/
            switch (Sort)
            {
                case "Всички":
                    all = all.OrderByDescending(x => x.CreatedOn);
                    break;
                case "Одобрени":
                    all = all.Where(x => x.isApproved == ApproveType.Success);
                    break;
                case "Архивирани":
                    all = all.Where(x => x.isArchived == true);
                    break;
                default: all = all.OrderByDescending(x => x.CreatedOn);
                    break;
            }

            if (await all.AnyAsync()) 
            {
                int count = await all.CountAsync().ConfigureAwait(false);
                Pager = new Pager(count, currentPage);

                var result = all
                .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                .Take(Pager.PageSize);

                JobsList = result;
            }
            else JobsList = null;

            return Page();
        }


        //[Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
        public async Task<IActionResult> OnGetCompaniesAsync(int currentPage = 1, string Sort = null)
        {
            TempData["ReturnUrl"] = Url.Page("List", "Companies", new { currentPage = currentPage, Sort = Sort, area = "Identity" });

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

            var all = _companyService.GetAllAsNoTracking();

            if (user.Role.Equals(Roles.Employer) || user.Role.Equals(Roles.Recruiter))
            {
                all = all.Where(x => x.PosterId == user.Id || x.Admin1_Id == user.Id || x.Admin2_Id == user.Id || x.Admin3_Id == user.Id);
            }

            all = all.Select(x => new Company
                {
                    Id = x.Id,
                    Title = x.Title,
                    Date = x.Date,
                    Logo = _CompanyPathShow + x.Logo,
                    isApproved = x.isApproved
                });

            switch (Sort)
            {
                case "Всички":
                    all = all.OrderByDescending(x => x.Date);
                    break;
                case "Одобрени":
                    all = all.Where(x => x.isApproved == ApproveType.Success);
                    break;
                default:
                    all = all.OrderByDescending(x => x.Date);
                    break;
            }

            if (await all.AnyAsync())
            {
                int count = await all.AsNoTracking().Distinct().CountAsync();
                Pager = new Pager(count, currentPage);

                var result = all
                .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                .Take(Pager.PageSize);

                CompaniesList = result.ToAsyncEnumerable();
            }
            else CompaniesList = null;

            return Page();
        }

        //[Authorize(Roles = "Admin, Moderator, Contestant, User")]
        public async Task<IActionResult> OnGetContestantsAsync(int currentPage = 1, string Sort = null)
        {
            TempData["ReturnUrl"] = Url.Page("List", "Contestants", new { currentPage = currentPage, Sort = Sort, area = "Identity" });

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

            var all = _contestantService.GetAllAsNoTracking();
           // var isPromotionExists = _promotionService.IsPromotionExists();

            if (user.Role.Equals(Roles.Contestant) || user.Role.Equals(Roles.User))
            {
                all = all.Where(x => x.PosterID == user.Id);
            }
             
                all = all.Select(x => new Entities.Models.Contestant
               {
                   Id = x.Id,
                   FullName = x.FullName,
                   CreatedOn = x.CreatedOn,
                   ExpiredOn = x.ExpiredOn,
                   isArchived = x.isArchived,
                   isApproved = x.isApproved,
                   PremiumPackage = x.PremiumPackage,
                   //isPromoteVerified = _promotionService.IsPromotionExists(PostType.Contestant, x.Id),
                   Logo = x.Logo != null && x.Logo.Contains("https://") ? x.Logo : (_UserPathShow + x.Logo)
            });;

            switch (Sort)
            {
                case "Всички":
                    all = all.OrderByDescending(x => x.CreatedOn);
                    break;
                case "Одобрени":
                    all = all.Where(x => x.isApproved == ApproveType.Success);
                    break;
                case "Архивирани":
                    all = all.Where(x => x.isArchived == true);
                    break;
                default:
                    all = all.OrderByDescending(x => x.CreatedOn);
                    break;
            }

            if (await all.AnyAsync())
            {
                int count = await all.AsNoTracking().Distinct().CountAsync();
                Pager = new Pager(count, currentPage);

                var result = all
                .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                .Take(Pager.PageSize);

                ContestantsList = result.AsAsyncEnumerable();
            }
            else ContestantsList = null;

            return Page();
        }

        //[Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]

        public PartialViewResult OnGetDeleteJobAsync(int id)
        {
            var model = new ListModel(_config, _userManager, _baseService, _spJobsService, _JobsService, _companyService, _contestantService, _promotionService) 
            {
                Model = new DeleteViewModel
                {
                    Id = id,
                    Handler = "DeleteJob"
                }
            };

            return PartialView("Partials/_Delete", model);
        }
        public PartialViewResult OnGetDeleteCompanyAsync(int id)
        {
            var model = new ListModel(_config, _userManager, _baseService, _spJobsService, _JobsService, _companyService, _contestantService, _promotionService)
            {
                Model = new DeleteViewModel
                {
                    Id = id,
                    Handler = "DeleteCompany"
                }
            };

            return PartialView("Partials/_Delete", model);
        }
        public PartialViewResult OnGetDeleteContestantAsync(int id)
        {
            var model = new ListModel(_config, _userManager, _baseService, _spJobsService, _JobsService, _companyService, _contestantService, _promotionService)
            {
                Model = new DeleteViewModel
                {
                    Id = id,
                    Handler = "DeleteContestant"
                }
            };

            return PartialView("Partials/_Delete", model);
        }
        [NonAction]
        public virtual PartialViewResult PartialView(string viewName, object model)
        {
            ViewData.Model = model;

            return new PartialViewResult()
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
        }


        public async Task<IActionResult> OnPostDeleteJobAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var job = await _spJobsService.GetByIdAsync<Entities.Models.Jobs>(Model.Id);

            if (job == null)
            {
               return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }
            if (!job.Name.Equals(Model.Password))
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "моля опитайте отново.", 5000);
                return Redirect("~/Identity/List/Jobs");
            }
            var company = await _companyService.GetByIdAsync(job.CompanyId);

            if (user.Id == job?.PosterID || user.Id != company?.Admin1_Id || user.Id != company?.Admin2_Id || user.Id != company?.Admin3_Id || user.Role.Equals(Roles.Admin) || user.Role.Equals(Roles.Moderator))
            {
                var result = await _spJobsService.CRUD(new { Id = Model.Id }, JobCrudActionEnum.Delete, false, null, null);
                if (result.Success)
                {
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "изтрихте обявата си.", 2000);
                    return Redirect("~/Identity/List/Jobs");
                }
            }
             return Page();
        }

        public async Task<IActionResult> OnPostDeleteCompanyAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var entity = await _companyService.GetByIdAsync(Model.Id);
            if (entity == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }
            if (!entity.Title.Equals(Model.Password))
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "моля опитайте отново.", 5000);
                return Redirect("~/Identity/List/Companies");
            }

            if (user.Id == entity?.PosterId || user.Role.Equals(Roles.Admin) || user.Role.Equals(Roles.Moderator))
            {
                var result = await _companyService.Delete(Model.Id);
                if (result.Success)
                {
                    await _JobsService.DeleteAllBy(Model.Id, null);
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "изтрихте фирмата си.", 2000);

                    return Redirect("~/Identity/List/Companies");
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteContestantAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var contestant = await _contestantService.GetByIdAsync(Model.Id);
            if (contestant == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (!contestant.FullName.Equals(Model.Password))
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "моля опитайте отново.", 5000);
                return Redirect("~/Identity/List/Contestants");
            }
            if (user.Id == contestant?.PosterID || user.Role.Equals(Roles.Admin) || user.Role.Equals(Roles.Moderator))
            {
                var result = await _contestantService.Delete(Model.Id);
                if (result.Success)
                {
                    var count = await _contestantService.GetCountByUser(user);
                    if (count <= 0)
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Contestant");
                        await _userManager.AddToRoleAsync(user, "User");
                        await _userManager.UpdateAsync(user);
                    }
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "изтрихте кандидатурата си.", 2000);

                    return Redirect("~/Identity/List/Contestants");
                }
            }

             return Page();
        }
    }
}