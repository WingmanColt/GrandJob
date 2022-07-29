namespace HireMe.Areas.Identity.Pages.MyFiles
{
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Moderator, Contestant, User")]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IFilesService _FilesService;
        private readonly IBaseService _baseService;

        private readonly string _FilePath;

        public IndexModel(
            IConfiguration config,
            UserManager<User> userManager,
            IFilesService FilesService,
            IBaseService baseService)
        {
            _userManager = userManager;
            _FilesService = FilesService;
            _baseService = baseService;

            _FilePath = config.GetValue<string>("CVPaths:myFilesCVPath");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : FilesInputModel { }
        public IAsyncEnumerable<Files> Result { get; set; }
        public int Count { get; set; }

        public User UserEntity { get; set; }

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

            UserEntity = user;
            Result = _FilesService.GetAllBy(user);
            Count = await _FilesService.GetFilesByUserCount(user.Id);   
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.FormFile != null)
            {
                Input.FileId = await _baseService.UploadFileAsync(Input.FormFile, null, null, FileType.MyFilesCV, user);

                if (Input.FileId is not null)
                {
                    var lenght = Input.FormFile.FileName.Length > 40 ? Input.FormFile.FileName.Length - 30 : Input.FormFile.FileName.Length;
                    OperationResult result = await _FilesService.Create(Input.FormFile.FileName.Substring(0, lenght), null, Input.FileId, user);

                    if (result.Success)
                        _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "качване.", 2000);
                    else _baseService.ToastNotify(ToastMessageState.Error, "Грешка", result.FailureMessage, 6000);
                }

            }

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            UserEntity = user;

            var entity = await _FilesService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (user.Email != entity.UserId)
            {
                return RedirectToPage("/Account/Errors/AccessDeniedContent", new { Area = "Identity" });
            }

            OperationResult result = await _FilesService.Delete(entity);

            string folderClearedName = Path.Combine(_FilePath, StringHelper.Filter(user?.Email));
            bool systemDeleted =  _baseService.Delete(folderClearedName);

            if (result.Success && systemDeleted)
                    _baseService.ToastNotify(ToastMessageState.Info, "Успешно", "файлът е премахнат.", 2000);
                else
                    await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "файлът не се изтри.","Files/Index", 5000);

            return RedirectToPage();
        }

    }
}