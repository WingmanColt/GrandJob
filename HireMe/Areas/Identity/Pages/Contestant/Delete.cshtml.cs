namespace HireMe.Areas.Identity.Pages.Contestant
{
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using HireMe.ViewModels.Contestants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    [Authorize(Roles = "Admin, Moderator, Contestant")]
    public partial class DeleteModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IBaseService _baseService;
        private readonly IContestantsService _contestantsService;

        public DeleteModel(
            IBaseService baseService,
        IContestantsService contestantsService,
            UserManager<User> userManager)
        {
            _baseService = baseService;
            this._contestantsService = contestantsService;
            this._userManager = userManager;
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
            var contestant = await _contestantsService.GetByIdAsync(id);
            if (contestant == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != contestant.PosterID && !user.Role.Equals(Roles.Admin) && !user.Role.Equals(Roles.Moderator))
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await this._contestantsService.Delete(id);
            if(result.Success)
            {
                await _userManager.RemoveFromRoleAsync(user, "Contestant");
                await _userManager.AddToRoleAsync(user, "User");
                await _userManager.UpdateAsync(user);

                _baseService.ToastNotify(ToastMessageState.Warning, "Внимание", "Обявата е премахната успешно.", 2000);

                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    RedirectToPage("/Identity/Contestant", new { Area = "Identity" });
            }

            return Page();
        }

    }
}
