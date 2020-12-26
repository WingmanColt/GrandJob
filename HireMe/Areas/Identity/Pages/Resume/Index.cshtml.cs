namespace HireMe.Areas.Identity.Pages.Resume
{
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using HireMe.ViewModels.Resume;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IResumeService _resumeService;
        private readonly IBaseService _baseService;
        private readonly string _FilePath;

        public IndexModel(
            IConfiguration config,
            UserManager<User> userManager,
            IResumeService resumeService,
            IBaseService baseService)
        {
            _userManager = userManager;
            _resumeService = resumeService;
            _baseService = baseService;

            _FilePath = config.GetValue<string>("StoredFilesPath");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : CreateResumeInputModel { }

        public async Task<IActionResult> OnGetAsync()
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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.FormFile != null)
            {
                Input.FileId = await _baseService.UploadFileAsync(Input.FormFile, null, user);


                OperationResult result = await _resumeService.Create(Input.FormFile.FileName, Input.FileId, user);

                if (result.Success)            
                 _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "файлът е в системата.", 2000);
               else _baseService.ToastNotify(ToastMessageState.Error, "Грешка", result.FailureMessage, 6000);
            }

            return Redirect("/Identity/Resume/Index");
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var entity = await _resumeService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Id != entity.UserId)
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

                var entId = entity.FileId;
                OperationResult result = await _resumeService.Delete(id);
                var systemDeleted =  _baseService.Delete($"{_FilePath}\\{entId}");

                if (result.Success && systemDeleted)
                {
                    _baseService.ToastNotify(ToastMessageState.Info, "", "файлът е премахнат.", 2000);
                    return Redirect("/Identity/Resume/Index");
                }else _baseService.ToastNotifyLog(user, ToastMessageState.Error, "", "файлът не се изтри от системата. Моля свържете се с админ","Resume/Index", 5000);

            return Page();
        }

    }
}