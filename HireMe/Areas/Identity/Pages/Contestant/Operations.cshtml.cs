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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    [PreventDuplicateRequest]
    [Authorize(Roles = "Admin, Moderator, Contestant, User")]
    public partial class OperationsModel : PageModel
    {
        private readonly IBaseService _baseService;
        private readonly UserManager<User> _userManager;
        private readonly IContestantsService _contestantService;
        private readonly ICategoriesService _categoriesService;
        private readonly ILocationService _locationService;
        private readonly ILanguageService _langService;
        private readonly ISkillsService _skillsService;
        private readonly INotificationService _notifyService;

        private readonly string _FilePath;
        private readonly string _ImagePath;
        public OperationsModel(
            IConfiguration config,
            IBaseService baseService,
            ICategoriesService categoriesService,
            ILocationService locationService,
            IContestantsService contestantService,
            ILanguageService langService,
            ISkillsService skillsService,
            INotificationService notifyService,
            UserManager<User> userManager)
        {
            _baseService = baseService;
            _categoriesService = categoriesService;
            _contestantService = contestantService;
            _locationService = locationService;
            _langService = langService;
            _skillsService = skillsService;
            _userManager = userManager;
            _notifyService = notifyService;

            _FilePath = config.GetValue<string>("MySettings:ResumeUrl");
            _ImagePath = config.GetValue<string>("MySettings:UserPicturePath");
        }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : CreateContestantInputModel
        {
            public string ImagePath { get; set; }

            public string resumeFullPath { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ReturnUrl = Url.Content("~/");

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

            LoadData(Input?.WorkType, Input?.LanguagesId, Input?.userSkillsId);

            if (id > 0 && contestant is not null && user.Id == contestant?.PosterID)
            {
                Input = new InputModel
                {
                    Id = contestant.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    LocationId = contestant.LocationId,
                    WorkType = contestant.WorkType,
                    Description = contestant.Description,
                    Genders = contestant.Genders,
                    Age = contestant.Age,
                    Speciality = contestant.Speciality,
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
                    LanguagesId = contestant.LanguagesId,
                    Views = contestant.Views,
                    Rating = contestant.Rating,
                    VotedUsers = contestant.VotedUsers
                };

                Input.resumeFullPath = _FilePath + contestant.ResumeFileId;

                User conUser = await _userManager.FindByIdAsync(contestant.PosterID);
                Input.ImagePath = (conUser.isExternal ? null : _ImagePath) + conUser.PictureName;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            ReturnUrl = Url.Content("~/");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var contestant = await _contestantService.GetByIdAsync(id);

            LoadData(Input?.WorkType, Input?.LanguagesId, Input?.userSkillsId);

            Input.Id = id;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.FormFile != null)
                Input.ResumeFileId = await _baseService.UploadFileAsync(Input.FormFile, contestant.ResumeFileId, user);

            OperationResult result;

            if (id > 0 && contestant is not null && user.Id == contestant?.PosterID)
            {
                Input.FullName = $"{user.FirstName} {user.LastName}";
                result = await this._contestantService.Update(Input, user);

                if (result.Success)
                {
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешна", "операция.", 2000);
                    await _notifyService.CreateForAdmins("Има кандидатура, който е редактирана и е в процес на изчакване за одобрение", $"", DateTime.Now, NotifyType.Warning, "far fa-clock", user.Id).ConfigureAwait(false);
                }
            }

            if (contestant is null)
            {
                Input.isArchived = false;
                result = await this._contestantService.Create(Input, user);

                if (result.Success)
                {
                    await _userManager.AddToRoleAsync(user, "Contestant");
                    await _userManager.RemoveFromRoleAsync(user, "User");
                    user.Role = Roles.Contestant;
                    await _userManager.UpdateAsync(user);

                    await _notifyService.CreateForAdmins("Има нова кандидатура, който е в процес на изчакване за одобрение", $"", DateTime.Now, NotifyType.Warning, "far fa-clock", user.Id).ConfigureAwait(false);
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешна", "операция.", 2000);
                    return Redirect(ReturnUrl);
                }
            }

            return RedirectToPage();
        }

        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }
        public IAsyncEnumerable<Language> Languages { get; set; }
        public IAsyncEnumerable<Skills> Skills { get; set; }
        public string[] Worktypes { get; set; }

        private void LoadData(string workType, string languages, string skills)
        {
            AllLocations = _locationService.GetAllSelectList();
            AllCategories = _categoriesService.GetAllSelectList();

            Languages = _langService.GetAll<Language>(languages, false);
            Skills = _skillsService.GetAll<Skills>(skills, false);

            Worktypes = workType?.Split(',');
        }

    }
}
