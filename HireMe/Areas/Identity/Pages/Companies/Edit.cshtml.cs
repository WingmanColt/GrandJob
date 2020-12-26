namespace HireMe.Areas.Identity.Pages.Companies
{
    using HireMe.Core.Helpers;
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using HireMe.Utility;
    using HireMe.ViewModels.Company;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    [PreventDuplicateRequest]
    [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
    public partial class EditModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly IBaseService _baseService;

        private readonly UserManager<User> _userManager;
        private readonly ICompanyService _companyService;
        private readonly ILocationService _locationService;
        private readonly string _ImagePathShow;

        public EditModel(
            IConfiguration config,
            IBaseService baseService,
            UserManager<User> userManager,
            ICompanyService companyService,
            ILocationService locationService)
        {
            _config = config;
            _baseService = baseService;
            _userManager = userManager;
            _companyService = companyService;
            _locationService = locationService;

            _ImagePathShow = config.GetValue<string>("MySettings:SiteImageUrl");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : CreateCompanyInputModel
        {
            public string PictureFullPath { get; set; }
         
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
            var company = await _companyService.GetByIdAsync(id);
            if (company == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != company.PosterId && !user.Role.Equals(Roles.Admin) && !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            LoadData();

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
                Admin3_Id = company.Admin3_Id
            };

            Input.PictureFullPath = _ImagePathShow + Input.Logo;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var company = await _companyService.GetByIdAsync(id);
            if (company == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != company.PosterId && !user.Role.Equals(Roles.Admin) && !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            LoadData();

            Input.Id = id;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.FormFile != null)
            {
                Input.Logo = await _baseService.UploadImageAsync(Input.FormFile, company.Logo, user);
            }
            if (Input.Admin2_Id != null)
            {
                var recruiterUser_2 = await _userManager.FindByNameAsync(Input.Admin2_Id);
                await _userManager.AddToRoleAsync(recruiterUser_2, "Recruiter");

                recruiterUser_2.Role = Roles.Recruiter;
            }

            if (Input.Admin3_Id != null)
            {
                var recruiterUser_3 = await _userManager.FindByNameAsync(Input.Admin3_Id);
                await _userManager.AddToRoleAsync(recruiterUser_3, "Recruiter");

                recruiterUser_3.Role = Roles.Recruiter;
            }

            OperationResult result = await this._companyService.Update(Input, true, user);

            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Детайлите ви са актулизирани.", 2000);
                return Redirect($"/Company/Details/{id}");
            }

            return Page();
        }

        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }
        private void LoadData()
        {
            AllLocations = _locationService.GetAllSelectList();                
        }

    }
}
