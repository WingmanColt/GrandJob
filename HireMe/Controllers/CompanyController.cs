using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly ICompanyService _companyService;
        private readonly ILocationService _locationService;
        private readonly IBaseService _baseService;

        public CompanyController(
            IBaseService baseService,
            UserManager<User> userManager,
            ICompanyService companyService,
            ILocationService locationService)
        {
            _baseService = baseService;
            _userManager = userManager;
            _companyService = companyService;
            _locationService = locationService;
        }


        [Authorize(Roles = "Admin, Moderator, Employer")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            if (user.AccountType == 0)
            {
                return Redirect("/Identity/Account/Manage/Pricing");
            }

            if (!user.profileConfirmed)
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "Моля попълнете личните си данни преди да продължите.", 7000);
                return Redirect("/Identity/Account/Manage/EditProfile");
            }
            CreateCompanyInputModel viewModel = new CreateCompanyInputModel();
            viewModel.AllLocations = _locationService.GetAllSelectList();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator, Employer")]
        public async Task<IActionResult> Create(CreateCompanyInputModel Input)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }


            if (!ModelState.IsValid)
            {
                Input.AllLocations = _locationService.GetAllSelectList();
                return View(Input);
            }


            if (Input.FormFile != null)
                Input.Logo = await _baseService.UploadImageAsync(Input.FormFile, null, user);

            if (Input.Admin1_Id != null)
            {
                var recruiterUser_1 = await _userManager.FindByNameAsync(Input.Admin1_Id);
                await _userManager.AddToRoleAsync(recruiterUser_1, "Recruiter");

                recruiterUser_1.Role = Roles.Recruiter;
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

            var result = await _companyService.Create(Input, true, user);


            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Warning, "Внимание", "Моля изчакайте заявката ви да се прегледа от администратор.", 7000);
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Фирмата ви е добавена.", 5000);
                return Redirect($"/identity/companies/index");
            }

            return View(Input);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var company = await _companyService.GetByIdAsyncMapped(id);

            if (company == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
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
                var PosterId = await _userManager.FindByIdAsync(company.PosterId);

                if (!(PosterId is null))
                {
                    
                    switch (T)
                    {
                        case ApproveType.Waiting:
                            await _notifyService.Create("Моля редактирайте вашата фирма отново с коректни данни.", "identity/companies/index", DateTime.Now, NotifyType.Warning, "fas fa-sync-alt", PosterId);
                            break;
                        case ApproveType.Rejected:
                            await _notifyService.Create("Последно добавената ви фирма е отхвърлена.", "identity/companies/index", DateTime.Now, NotifyType.Danger, "fas fa-ban", PosterId);
                            break;
                        case ApproveType.Success:
                            await _notifyService.Create("Последно добавената ви фирма е одобрена.", "identity/companies/index", DateTime.Now, NotifyType.Information, "fas fa-check", PosterId);
                            break;
                    }
                   
                }
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    RedirectToPage("/Identity/Companies", new { Area = "Identity" });
            }

            return View();
        }


        [Authorize]
        public async Task<ActionResult> UpdateFavourite(int id, string returnUrl, [FromServices] IFavoritesService _favoriteService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            if (!(await _favoriteService.isInFavourite(user, PostType.Company, id)))
                await _favoriteService.AddToFavourite(user, PostType.Company, id.ToString());
            else
                await _favoriteService.RemoveFromFavourite(user, PostType.Company, id.ToString());

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
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