namespace HireMe.Areas.Identity.Pages.Companies
{
    using HireMe.Controllers;
    using HireMe.Core.Helpers;
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using HireMe.Utility;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    [PreventDuplicateRequest]
    [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
    public partial class OperationsModel : PageModel
    {
        private readonly IBaseService _baseService;

        private readonly UserManager<User> _userManager;
        private readonly ICompanyService _companyService;
        private readonly ILocationService _locationService;
        private readonly ICategoriesService _categoriesService;
        private readonly INotificationService _notifyService;

        private readonly string _ImagePathShow;
        private readonly string _GalleryPathShow;

        public OperationsModel(
            IConfiguration config,
            IBaseService baseService,
            UserManager<User> userManager,
            ICompanyService companyService,
            ILocationService locationService,
            ICategoriesService categoriesService,
            INotificationService notifyService)
        {
            _baseService = baseService;
            _userManager = userManager;
            _companyService = companyService;
            _locationService = locationService;
            _categoriesService = categoriesService;
            _notifyService = notifyService;

            _ImagePathShow = config.GetValue<string>("MySettings:CompanyImageUrl");
            _GalleryPathShow = config.GetValue<string>("MySettings:GalleryImageUrl");
        }
        public string ReturnUrl { get; set; }
        public List<IFormFile> files { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : CreateCompanyInputModel
        {
            public string PictureFullPath { get; set; }
            public string GalleryFullPath { get; set; }
            public IAsyncEnumerable<string> MyGalleryImages { get; set; }

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

            LoadData();

            var company = await _companyService.GetByIdAsync(id);

            if (id > 0 && company is not null && user.Id == company?.PosterId)
            {
                Input = new InputModel
                {
                    Id = company.Id,
                    Email = company.Email,
                    PhoneNumber = company.PhoneNumber,
                    Title = company.Title,
                    About = company.About,
                    Private = company.Private,
                    Logo = company.Logo,
                    LocationId = company.LocationId,
                    Adress = company.Adress,
                    Website = company.Website,
                    Facebook = company.Facebook,
                    Linkdin = company.Linkdin,
                    Twitter = company.Twitter,
                    isAuthentic_EIK = company.isAuthentic_EIK,
                    Admin1_Id = company.Admin1_Id,
                    Admin2_Id = company.Admin2_Id,
                    Admin3_Id = company.Admin3_Id,
                    isApproved = company.isApproved,
                    Promotion = company.Promotion,
                    MyGalleryImages = company.GalleryImages?.Split(',').ToAsyncEnumerable(),
                    Rating = company.Rating

                };

                Input.PictureFullPath = _ImagePathShow + (company.Logo is not null? company.Logo : Input.Logo);
                Input.GalleryFullPath = Path.Combine(_GalleryPathShow, StringHelper.Filter(company.Email));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string returnUrl)
        {
            ReturnUrl = Url.Content("~/");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            LoadData();

            var company = await _companyService.GetByIdAsync(id);

            if (Input.FormFile is not null)
            {
                Input.Logo = await _baseService.UploadImageAsync(Input.FormFile, company?.Logo, true, user);
            }
            if (Input.files is not null)
            {
                Input.GalleryImages = await _baseService.MultipleUploadFileAsync(Input.Email, 500, 500, Input.files);
            }
            if (Input.Admin1_Id is not null)
            {
                var recruiterUser_1 = await _userManager.FindByNameAsync(Input.Admin1_Id);
                await _userManager.AddToRoleAsync(recruiterUser_1, "Recruiter");

                recruiterUser_1.Role = Roles.Recruiter;
            }

            if (Input.Admin2_Id is not null)
            {
                var recruiterUser_2 = await _userManager.FindByNameAsync(Input.Admin2_Id);
                await _userManager.AddToRoleAsync(recruiterUser_2, "Recruiter");

                recruiterUser_2.Role = Roles.Recruiter;
            }

            if (Input.Admin3_Id is not null)
            {
                var recruiterUser_3 = await _userManager.FindByNameAsync(Input.Admin3_Id);
                await _userManager.AddToRoleAsync(recruiterUser_3, "Recruiter");

                recruiterUser_3.Role = Roles.Recruiter;
            }

            OperationResult result;
            if (id > 0 && company is not null && user.Id == company?.PosterId)
            {
                Input.Id = id;
                result = await this._companyService.Update(Input, true, user);

                if (result.Success)
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешна", "операция.", 2000);
            }
            if (company is null)
            {
                result = await this._companyService.Create(Input, true, user);

                if (result.Success)
                {
                    await _notifyService.CreateForAdmins("Има фирма в процес на изчакване за одобрение", $"", DateTime.Now, NotifyType.Warning, "far fa-clock", user.Id).ConfigureAwait(false);
                    _baseService.ToastNotify(ToastMessageState.Success, "Успешна", "операция.", 2000);
                    return Redirect(ReturnUrl);
                }
            }

            return RedirectToPage();
        }

        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }
        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        private void LoadData()
        {
            AllLocations = _locationService.GetAllSelectList();
            AllCategories = _categoriesService.GetAllSelectList();

        }

    }
}
