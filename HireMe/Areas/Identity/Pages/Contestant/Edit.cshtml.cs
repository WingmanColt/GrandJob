namespace HireMe.Areas.Identity.Pages.Contestant
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
    [Authorize(Roles = "Admin, Moderator, Contestant")]
    public partial class EditModel : PageModel
    {
        private readonly IBaseService _baseService;
        private readonly UserManager<User> _userManager;
        private readonly IContestantsService _contestantService;
        private readonly ICategoriesService _categoriesService;
        private readonly ILocationService _locationService;
        private readonly ILanguageService _langService;
        private readonly ISkillsService _skillsService;

        private readonly string _FilePath;
        private readonly string _ImagePath;
        public EditModel(
            IConfiguration config,
            IBaseService baseService,
            ICategoriesService categoriesService,
            ILocationService locationService,
            IContestantsService contestantService,
            ILanguageService langService,
            ISkillsService skillsService,
            UserManager<User> userManager)
        {
            _baseService = baseService;
            _categoriesService = categoriesService;
            _contestantService = contestantService;
            _locationService = locationService;
            _langService = langService;
            _skillsService = skillsService;
            _userManager = userManager;

            _FilePath = config.GetValue<string>("MySettings:ResumeUrl");
            _ImagePath = config.GetValue<string>("MySettings:SiteImageUrl");
        }


        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : CreateContestantInputModel
        {
            public string ImagePath { get; set; }

            public string resumeFullPath { get; set; }
        }

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
            var contestant = await _contestantService.GetByIdAsync(id);

            if (contestant == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != contestant.PosterID && !user.Role.Equals(Roles.Admin) && !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            LoadData(contestant);

            Input = new InputModel
            {
                Id = contestant.Id,
                FullName = contestant.FullName,
                LocationId = contestant.LocationId,
                WorkType = contestant.WorkType,
                Description = contestant.Description,
                Genders = contestant.Genders,
                Age = contestant.Age,
                About = contestant.About,
                Experience = contestant.Experience,
                SalaryType = contestant.SalaryType,
                CategoryId = contestant.CategoryId,
                payRate = contestant.payRate,
                profileVisiblity = contestant.profileVisiblity,
                Website = contestant.Website,
                Portfolio = contestant.Portfolio,
                Facebook = contestant.Facebook,
                Linkdin = contestant.Linkdin,
                Twitter = contestant.Twitter,
                Github = contestant.Github,
                Dribbble = contestant.Dribbble,
                ResumeFileId = contestant.ResumeFileId,
                userSkillsId = contestant.userSkillsId,
                LanguagesId = contestant.LanguagesId
            };

            Input.resumeFullPath = _FilePath + contestant.ResumeFileId;

            User conUser = await _userManager.FindByIdAsync(contestant.PosterID);
            Input.ImagePath = (conUser.isExternal ? null : _ImagePath) + conUser.PictureName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var contestant = await _contestantService.GetByIdAsync(id);

            if (contestant == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != contestant.PosterID && !user.Role.Equals(Roles.Admin) && !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            LoadData(contestant);

            Input.Id = id;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.FormFile != null)
                Input.ResumeFileId = await _baseService.UploadFileAsync(Input.FormFile, contestant.ResumeFileId, user);

            OperationResult result = await this._contestantService.Update(Input, user);
            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Обявата ви е актулизирана.", 2000);
                return Redirect($"/Identity/Contestant/Index");

            }
            return RedirectToPage();
        }
        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }
        public IAsyncEnumerable<Language> Languages { get; set; }
        public IAsyncEnumerable<Skills> Skills { get; set; }
        public string[] Worktypes { get; set; }
        private void LoadData(Contestant contestant)
        {
            AllLocations = _locationService.GetAllSelectList();
            AllCategories = _categoriesService.GetAllSelectList();

            Languages = _langService.GetAll(contestant.LanguagesId);
            Skills = _skillsService.GetAll<Skills>(contestant.LanguagesId, false);

            Worktypes = contestant.WorkType?.Split(',');
        }

    }
}
