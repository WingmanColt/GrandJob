namespace HireMe.Areas.Identity.Pages.Companies
{
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using HireMe.ViewModels.Company;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    [Authorize(Roles = "Admin, Moderator, Employer")]
    public partial class DeleteModel : PageModel
    {
        private readonly IBaseService _baseService;
        private readonly ICompanyService _companyService;
        private readonly IJobsService _jobsService;
        private readonly UserManager<User> _userManager;

        public DeleteModel(
            IBaseService baseService,
            ICompanyService companyService,
            IJobsService jobsService,
            UserManager<User> userManager)
        {
            _baseService = baseService;
            _companyService = companyService;
            _jobsService = jobsService;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Моля въведете вашата парола.")]
            [Display(Name = "Парола")]
            [DataType(DataType.Password, ErrorMessage = "Грешна парола.")]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
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

            if (user.Id != entity.PosterId && !user.Role.Equals(Roles.Admin) && !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Грешна парола.");
                    return Page();
                }
                       
            var result = await _companyService.Delete(id);
            if (result.Success)
            {
                await _jobsService.DeleteAllBy(id, null);
                _baseService.ToastNotify(ToastMessageState.Warning, "Внимание", "Фирмата е премахната успешно.", 5000);


                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    RedirectToPage("/Identity/Companies", new { Area = "Identity" });
            }

            return Page();         
        }

    }
}
