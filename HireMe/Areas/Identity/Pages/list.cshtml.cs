
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages
{
    [Authorize]
    public partial class ListModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        private readonly IBaseService _baseService;
        private readonly IJobsService _jobsService;
        private readonly ICompanyService _companyService;
        private readonly IContestantsService _contestantService;

        private readonly string _CompanyPathShow;
        private readonly string _UserPathShow;

        public ListModel(
            IConfiguration config,
            UserManager<User> userManager,
            IBaseService baseService,
            IJobsService jobsService,
            ICompanyService companyService,
            IContestantsService contestantService)
        {
            _userManager = userManager;
            _baseService = baseService;
            _jobsService = jobsService;
            _companyService = companyService;
            _contestantService = contestantService;

            _CompanyPathShow = config.GetValue<string>("MySettings:CompanyImageUrl");
            _UserPathShow = config.GetValue<string>("MySettings:UserPicturePath");
        }

       // [BindProperty]
        public Pager Pager { get; set; }
        public User UserEntity { get; set; }

        public IAsyncEnumerable<Entities.Models.Jobs> JobsList { get; set; }
        public IAsyncEnumerable<Entities.Models.Company> CompaniesList { get; set; }
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

        [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
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

            var all = _jobsService.GetAllAsNoTracking();

            if (user.Role.Equals(Roles.Employer) || user.Role.Equals(Roles.Recruiter))
            {
                all = all.Where(x => x.PosterID == user.Id);
            }

             all = all.Select(x => new Entities.Models.Jobs
                 {
                                   Id = x.Id,
                                   Name = x.Name,
                                   CompanyLogo = _CompanyPathShow + x.CompanyLogo,
                                   CreatedOn = x.CreatedOn,
                                   ExpiredOn = x.ExpiredOn,
                                   isArchived = x.isArchived,
                                   isApproved = x.isApproved
                });
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
                int count = await all.AsNoTracking().Distinct().CountAsync();
                Pager = new Pager(count, currentPage);

                var result = all
                .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                .Take(Pager.PageSize);

               JobsList = result.ToAsyncEnumerable();
            }
            else JobsList = null;

            return Page();
        }


        [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
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

        [Authorize(Roles = "Admin, Moderator, Contestant, User")]
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
            if (user.Role.Equals(Roles.Contestant))
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
                   Logo = x.Logo != null && x.Logo.Contains("https://") ? x.Logo : (_UserPathShow + x.Logo)
            });

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

                ContestantsList = result.ToAsyncEnumerable();
            }
            else ContestantsList = null;

            return Page();
        }

        [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
        public async Task<IActionResult> OnPostDeleteJobAsync(int id, int currentPage = 1, string Sort = null)
        {
            Pager = new Pager(0, currentPage);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            var job = await _jobsService.GetByIdAsync(id);
            if (job == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }
            var company = await _companyService.GetByIdAsync(job.CompanyId);

            if (user.Id != job.PosterID || user.Id != company?.Admin1_Id || user.Id != company?.Admin2_Id || user.Id != company?.Admin3_Id || !user.Role.Equals(Roles.Admin) || !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "моля опитайте с друга парола.", 5000);
                return Page();
            }

            var result = await _jobsService.Delete(id);
            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "изтрихте обявата си.", 2000);

                if (!String.IsNullOrEmpty(TempData["ReturnUrl"] as string))
                    return Redirect(TempData["ReturnUrl"] as string);
                else
                    RedirectToPage("List", "Jobs", new { currentPage, Sort, Area = "Identity", });
            }

            return Page();
        }

        [Authorize(Roles = "Admin, Moderator, Employer")]
        public async Task<IActionResult> OnPostDeleteCompanyAsync(int id, int currentPage = 1, string Sort = null)
        {
            Pager = new Pager(0, currentPage);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            var entity = await _companyService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != entity.PosterId || !user.Role.Equals(Roles.Admin) || !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "моля опитайте с друга парола.", 5000);
                return Page();
            }

            var result = await _companyService.Delete(id);
            if (result.Success)
            {
                await _jobsService.DeleteAllBy(id, null);
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "изтрихте фирмата си.", 2000);

                if (!String.IsNullOrEmpty(TempData["ReturnUrl"] as string))
                    return Redirect(TempData["ReturnUrl"] as string);
                else
                    RedirectToPage("List", "Companies", new { currentPage, Sort, Area = "Identity", });
            }

            return Page();
        }

        [Authorize(Roles = "Admin, Moderator, Contestant")]
        public async Task<IActionResult> OnPostDeleteContestantsAsync(int currentPage = 1, string Sort = null)
        {
            Pager = new Pager(0, currentPage);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }
            var contestant = await _contestantService.GetByIdAsync(Input.Id);
            if (contestant == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != contestant.PosterID || !user.Role.Equals(Roles.Admin) || !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "моля опитайте с друга парола.", 5000);
                return Page();
            }

            var result = await _contestantService.Delete(Input.Id);
            if (result.Success)
            {
                var count = await _contestantService.GetCountByUserId(user);
                if (count <= 0)
                {
                    await _userManager.RemoveFromRoleAsync(user, "Contestant");
                    await _userManager.AddToRoleAsync(user, "User");
                    await _userManager.UpdateAsync(user);
                }
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "изтрихте кандидатурата си.", 2000);

                if (!String.IsNullOrEmpty(TempData["ReturnUrl"] as string))
                    return Redirect(TempData["ReturnUrl"] as string);
                else
                    RedirectToPage("List", "Contestants", new { currentPage, Sort, Area = "Identity", });
            }
          // return RedirectToPage("List", "Contestants", new { currentPage, Sort, Area = "Identity", });

             return Page();
        }
    }
}