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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    [PreventDuplicateRequest]
    [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
    public partial class OperationsModel : PageModel
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

        public OperationsModel(
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

            _ImagePathShow = config.GetValue<string>("MySettings:CompanyImageUrl");

        }


        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : CreateJobInputModel { }

        [BindProperty]
        public Company Company { get; set; }
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            ReturnUrl = Url.PageLink();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.AccountType == 0)
            {
                return RedirectToPage("/Account/Manage/Pricing", new { Area = "Identity" });
            }

            AllLocations = _locationService.GetAllSelectList();
            AllCategories = _categoriesService.GetAllSelectList();

            var job = await _jobsService.GetByIdAsync(id);
            LoadData(user, Input?.WorkType, Input?.LanguageId, Input?.TagsId);

            if (id > 0 && job is not null && user.Id == job.PosterID)
            {
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
                    ExprienceLevels = job.ExprienceLevels,
                    Views = job.Views,
                    ApplyCount = job.ApplyCount,
                    Rating = job.Rating,
                    isApproved = job.isApproved
                };

                Company = await _companyService.GetByIdAsync(Input.CompanyId);
                Input.CompanyLogo = _ImagePathShow + Company?.Logo;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int Id)
        {
           // ReturnUrl = Url.PageLink();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var job = await _jobsService.GetByIdAsync(Id);

            LoadData(user, Input?.WorkType, Input?.LanguageId, Input?.TagsId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //var more = HttpContext.Request.Form.Files;

            OperationResult result;

            Company = await _companyService.GetByIdAsync(Input.CompanyId);
            if (Company is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            Input.CompanyLogo = Company.Logo;

            if (Id > 0 && job is not null && user.Id == job.PosterID)
            {
                Input.Id = Id;

                result = await _jobsService.Update(Input, user);
                if (result.Success)
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешна", "операция.", 2000);
            }

            if (job is null)
            {
                result = await _jobsService.Create(Input, user);

                if (result.Success)
                {
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешна", "операция.", 2000);
                }
            }

            if(!String.IsNullOrEmpty(ReturnUrl))
                return Redirect(ReturnUrl);
            else
            return RedirectToPage();
        }

        public IAsyncEnumerable<Company> Companies { get; set; }
        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }
        public IAsyncEnumerable<Skills> Skills { get; set; }
        public IAsyncEnumerable<Language> Languages { get; set; }
        public string[] Worktypes { get; set; }

        private void LoadData(User user, string workType, string languages, string skills)
        {
            AllLocations = _locationService.GetAllSelectList();
            AllCategories = _categoriesService.GetAllSelectList();

            Companies = _companyService.GetAll(user);
            Languages = _langService.GetAll<Language>(languages, false);
            Skills = _skillsService.GetAll<Skills>(skills, false);

            Worktypes = workType?.Split(',');
        }
    }
}
