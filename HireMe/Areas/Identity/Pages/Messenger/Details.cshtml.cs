using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Messenger
{
    [Authorize]
    public partial class DetailsModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IBaseService _baseService;
        private readonly IMessageService _messageService;

        public DetailsModel(UserManager<User> userManager, IBaseService baseService, IMessageService messageService)
        {
            _userManager = userManager;
            _baseService = baseService;
            _messageService = messageService;
        }
        public int Id { get; set; }

        public User curretntUser { get; set; }
        public User sender { get; set; }
        public User receiver { get; set; }

        [BindProperty]
        public CreateMessageInputModel Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            curretntUser = await _userManager.GetUserAsync(User);

            if (curretntUser == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (!curretntUser.MessagesEnable)
            {
                return RedirectToPage("/Messenger/Enable", new { Area = "Identity" });
            }

            var entity = await _messageService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            // use instead viewmodel cuz razor page cannot map
            //  var content = await this._messageService.GetByIdMessage(id);


            ViewData["Message"] = entity;
            Id = id;

            sender = await _userManager.FindByIdAsync(entity.SenderId);
            receiver = await _userManager.FindByIdAsync(entity.ReceiverId);

            if (!entity.isRead)
            entity.isRead = true;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            curretntUser = await _userManager.GetUserAsync(User);

            if (curretntUser == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // use instead viewmodel cuz razor page cannot map
            var entity = await _messageService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            if (!entity.isRead)
                entity.isRead = true;

            ViewData["Message"] = entity;

            Id = id;
            sender = await _userManager.FindByIdAsync(entity.SenderId);
            receiver = await _userManager.FindByIdAsync(entity.ReceiverId);


            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetSendMessageAsync(int id)
        {
            curretntUser = await _userManager.GetUserAsync(User);

            if (curretntUser == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var entity = await _messageService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }


            ViewData["Message"] = entity;
            Id = id;
            sender = await _userManager.FindByIdAsync(entity.SenderId);
            receiver = await _userManager.FindByIdAsync(entity.ReceiverId);


            if (!entity.isRead)
                entity.isRead = true;

            return Page();
        }
        public async Task<IActionResult> OnPostSendMessageAsync(int id)
        {
            curretntUser = await _userManager.GetUserAsync(User);

            if (curretntUser == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var entity = await _messageService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToPage("/Account/Errors/NoEntity", new { Area = "Identity" });
            }

            ViewData["Message"] = entity;
            Id = id;
            sender = await _userManager.FindByIdAsync(entity.SenderId);
            receiver = await _userManager.FindByIdAsync(entity.ReceiverId);


            if (!ModelState.IsValid)
            {
                 _baseService.ToastNotify(ToastMessageState.Error, "Грешка", $"Моля спазвайте изискванията !", 3000);
                return Page();
            }

            var result = await this._messageService.Create(Message.Title, Message.Description, curretntUser.Id, entity.SenderId);
            if (result.Success)
            {
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Съобщението ви е изпратено.", 2000);
                return Redirect($"/Identity/Messenger/Sent");
            }
            else
            {
                _baseService.ToastNotify(ToastMessageState.Error, "Грешка", $"{result.FailureMessage}", 2000);
                return Page();
            }
        }

    }
}
