namespace HireMe.Areas.Identity.Pages.Jobs
{
    using HireMe.Core.Helpers;
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using HireMe.Utility;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    [PreventDuplicateRequest]
    [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
    public partial class EditModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IJobsService _jobsService;
        private readonly ICategoriesService _categoriesService;
        private readonly ILocationService _locationService;
        private readonly ICompanyService _companyService;
        private readonly ILanguageService _langService;
        private readonly ISkillsService _skillsService;
        private readonly IBaseService _baseService;
        private readonly string _ImagePathShow;

        public EditModel(
            UserManager<User> userManager,
            ICategoriesService categoriesService,
            ILocationService locationService,
            IJobsService jobsService,
            ICompanyService companyService,
            ILanguageService langService,
            ISkillsService skillsService,
            IConfiguration config,
            IBaseService baseService)
        {
            _userManager = userManager;
            _categoriesService = categoriesService;
            _locationService = locationService;
            _jobsService = jobsService;
            _companyService = companyService;
            _langService = langService;
            _skillsService = skillsService;
            _baseService = baseService;

            _ImagePathShow = config.GetValue<string>("MySettings:SiteImageUrl");

        }


        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : CreateJobInputModel { }

        [BindProperty]
        public Company Company { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.AccountType == 0)
            {
                return RedirectToPage("/Account/Manage/Pricing", new { Area = "Identity" });
            }

            var job = await _jobsService.GetByIdAsync(id);
            if (job == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != job.PosterID && !user.Role.Equals(Roles.Admin) && !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            LoadData(job, user);

            Input = new InputModel
            {
                Id = job.Id,
                Name = job.Name,
                LocationId = job.LocationId,
                WorkType = job.WorkType,
                Description = job.Description,
                Adress = job.Adress,
                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,
                SalaryType = job.SalaryType,
                CategoryId = job.CategoryId,
                CompanyId = job.CompanyId,
                LanguageId = job.LanguageId,
                TagsId = job.TagsId,
                isApproved = job.isApproved
            };

            Company = await _companyService.GetByIdAsync(Input.CompanyId);
            Input.CompanyLogo = _ImagePathShow + Company?.Logo;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var job = await _jobsService.GetByIdAsync(Id);
            if (job == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != job.PosterID && !user.Role.Equals(Roles.Admin) && !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            LoadData(job, user);

            if (!ModelState.IsValid)
            {
                return Page();
            }
            Input.Id = Id;
            OperationResult result = await _jobsService.Update(Input, user);
            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Обявата ви е актулизирана.", 2000);
                return Redirect($"/Identity/Jobs/Index");
            }

            return RedirectToPage();
        }

        public IAsyncEnumerable<Company> Companies { get; set; }
        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }
        public IAsyncEnumerable<Skills> Skills { get; set; }
        public IAsyncEnumerable<Language> Languages { get; set; }
        public string[] Worktypes { get; set; }

        private void LoadData(Jobs job, User user)
        {
            AllLocations = _locationService.GetAllSelectList();
            AllCategories = _categoriesService.GetAllSelectList();

            Companies = _companyService.GetAll(user);
            Languages = _langService.GetAll(job.LanguageId);
            Skills = _skillsService.GetAll<Skills>(job.TagsId, false);

            Worktypes = job.WorkType?.Split(',');
        }
    }
}
