using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Messenger
{
    [Authorize]
    public partial class ComposeModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IMessageService _messageService;
        private readonly IBaseService _baseService;

        public ComposeModel(
            UserManager<User> userManager,
            IBaseService baseService,
            IMessageService messageService)
        {
            this._userManager = userManager;
            this._messageService = messageService;
            this._baseService = baseService;
        }

        [BindProperty]
        public CreateMessageInputModel Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.AccountType == 0)
            {
                return Redirect("/Identity/Account/Manage/Pricing");
            }
            if (!user.MessagesEnable)
            {
                return Redirect($"/Identity/Messenger/Enable");
            }
            if (!user.profileConfirmed)
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", "Моля попълнете личните си данни преди да продължите.", 20000);
                return Redirect($"/Identity/Account/Manage/EditProfile");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.FindByNameAsync(user.UserName);
            var receiverId = await _userManager.FindByNameAsync(Message.ReceiverId);

            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            // if (!ModelState.IsValid)
            // {
            //    return Page();
            // }         

            OperationResult result = await this._messageService.Create(Message.Title, Message.Description, userId.Id, receiverId.Id);

            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Изпратихте вашето съобщение.", 2000);
                //user.LastConntactId = Message.ReceiverId;
                return Redirect($"/Identity/Messenger/Index");
            }
            else _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", $"{result.FailureMessage}", Request.GetDisplayUrl(), 2000);


            return Page();
        }

    }

}
