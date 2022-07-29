namespace HireMe.Areas.Identity.Pages.Jobs
{
    using HireMe.Core.Helpers;
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Services;
    using HireMe.Services.Interfaces;
    using HireMe.StoredProcedures.Enums;
    using HireMe.StoredProcedures.Interfaces;
    using HireMe.Utility;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    [PreventDuplicateRequest]
    [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
    public partial class OperationsModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IJobsService _jobsService;
        private readonly IspJobService _spJobService;
        private readonly ICategoriesService _categoriesService;
        private readonly ILocationService _locationService;
        private readonly ICompanyService _companyService;
        private readonly ILanguageService _langService;
        private readonly ISkillsService _skillsService;
        private readonly IBaseService _baseService;
        private readonly ILogService _logService;


        private readonly string _ImagePathShow;

        public OperationsModel(
            UserManager<User> userManager,
            ICategoriesService categoriesService,
            ILocationService locationService,
            IJobsService jobsService,
            IspJobService spJobService,
            ICompanyService companyService,
            ILanguageService langService,
            ISkillsService skillsService,
            IConfiguration config,
            IBaseService baseService,
            ILogService logService)
        {
            _userManager = userManager;
            _categoriesService = categoriesService;
            _locationService = locationService;
            _jobsService = jobsService;
            _spJobService = spJobService;
            _companyService = companyService;
            _langService = langService;
            _skillsService = skillsService;
            _baseService = baseService;
            _logService = logService;

            _ImagePathShow = config.GetValue<string>("MySettings:CompanyImageUrl");

        }

        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }

        public IAsyncEnumerable<Company> AllCompanies { get; set; }
        public IAsyncEnumerable<Skills> AllTags { get; set; }
        public IAsyncEnumerable<Language> AllLanguages { get; set; }

        public string[] Worktypes { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : CreateJobInputModel {

            [NotMapped]
            public PremiumPackage PremiumPackagePre { get; set; }
        }


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

            LoadData(user, Input?.WorkType, Input?.LanguageId, Input?.TagsId);

            Jobs job = await _spJobService.GetByIdAsync<Jobs>(id);

            if (id > 0 && job is not null && (user.Id == job?.PosterID || user.Role.Equals(Roles.Admin) || user.Role.Equals(Roles.Moderator)))
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
                    isApproved = job.isApproved,
                    Promotion = job.Promotion,
                    PremiumPackage = job.PremiumPackage,
                    PremiumPackagePre = job.PremiumPackage
                };

                LoadData(user, Input?.WorkType, Input?.LanguageId, Input?.TagsId);

                Company = await _companyService.GetByIdAsync(Input.CompanyId);
                Input.CompanyLogo = _ImagePathShow + Company?.Logo;
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            LoadData(user, Input?.WorkType, Input?.LanguageId, Input?.TagsId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var job = await _spJobService.GetByIdAsync<Jobs>(Id);

            OperationResult result;

            Company = await _companyService.GetByIdAsync(Input.CompanyId);
            if (Company is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            Input.CompanyLogo = Company.Logo;

            if (Id > 0 && job is not null && (user.Id == job?.PosterID || user.Role.Equals(Roles.Admin) || user.Role.Equals(Roles.Moderator)))
            {
                Input.Id = Id;

                result = await _spJobService.CRUD(Input, JobCrudActionEnum.Update, true, "NotMapped", user);

                if (result.Success)
                {
                    if (!Input.PremiumPackagePre.Equals(PremiumPackage.None))
                        return RedirectToPage("/Checkout/Index", new { package = (int)Input.PremiumPackagePre, productId = result.Id, PostType = (int)PostType.Job });
                    else
                    {
                        _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "редактирахте вашата обява.", 2000);
                        return Redirect("~/Identity/List/Jobs");
                    }
                }
                else await _logService.Create(result.FailureMessage, Input.Name, LogLevel.Danger, user?.UserName).ConfigureAwait(false);



            }

            if (job is null)
            {
                bool checkCount = await _spJobService.GetAllCountBy(new { PosterId = user.Id }) >= int.Parse(user.AccountType.GetShortName());
                if (checkCount)
                {
                    _baseService.ToastNotify(ToastMessageState.Warning, "Отказана операция", $"Имате право да добавяте само {user.AccountType.GetShortName()} обяви.", 2000);
                    return Redirect("~/Identity/List/Jobs");
                }
            
                result = await _spJobService.CRUD(Input, JobCrudActionEnum.Create, true, "NotMapped", user);

                if (result.Success)
                {
                    if (!Input.PremiumPackagePre.Equals(PremiumPackage.None))
                        return RedirectToPage("/Checkout/Index", new { package = (int)Input.PremiumPackagePre, productId = result.Id, PostType = (int)PostType.Job });
                    else
                    {
                        _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "добавихте обява.", 2000);
                        return Redirect("~/Identity/List/Jobs");
                    }
                }
                else await _logService.Create(result.FailureMessage, Input.Name, LogLevel.Danger, user?.UserName).ConfigureAwait(false);

            }

            return RedirectToPage();
        }


        private void LoadData(User user, string workType, string languages, string skills)
        {
            AllLocations = _locationService.GetAllSelectList();
            AllCategories = _categoriesService.GetAllSelectList();

            AllCompanies = _companyService.GetAll(user);
            AllLanguages = _langService.GetAll<Language>(languages, false);
            AllTags = _skillsService.GetAll<Skills>(skills, false);
            Worktypes = workType?.Split(',');
        }
    }
}
