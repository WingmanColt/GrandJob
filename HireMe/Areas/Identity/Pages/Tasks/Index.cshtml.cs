namespace HireMe.Areas.Identity.Pages.Tasks
{
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Services.Core.Interfaces;
    using HireMe.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ValidateAntiForgeryToken]
    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IBaseService _baseService;

        private readonly ITaskService _taskService;
        private readonly ICipherService _cipherService;
        private readonly IMessageService _messageService;

        public IndexModel(UserManager<User> userManager, 
            IBaseService baseService,
            ITaskService taskService,
            ICipherService cipherService,
            IMessageService messageService)
        {
            _userManager = userManager;
            _baseService = baseService;

            _taskService = taskService;
            _cipherService = cipherService;
            _messageService = messageService;

        }

        [BindProperty]
        public CreateMessageInputModel Message { get; set; }

        public Pager Pager { get; set; }

        public IAsyncEnumerable<Tasks> List { get; set; }
        public string Sort { get; set; }
        public string ReturnUrl { get; set; }

        public Tasks TaskEntity { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var all = _taskService.GetAllAsNoTracking()
                .Where(x => (x.SenderId == user.Id) || x.ReceiverId == user.Email);

            if (await all.AnyAsync())
            {
                int count = await all.AsQueryable().AsNoTracking().CountAsync();
                Pager = new Pager(count, currentPage);

                var result = all
                .OrderByDescending(x => x.Date)
                .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                .Take(Pager.PageSize);

                List = result.ToAsyncEnumerable();
            }
            else List = null;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }
            var entity = await _taskService.GetByIdAsync(id);
            if (entity == null)
            {
                return Redirect("/Identity/Account/Errors/NoEntity");
            }

            var result = await this._messageService.Create(entity.Title, Message.Description, user.Id, entity.SenderId);
            if (result.Success)
                _baseService.ToastNotify(ToastMessageState.Success, "Успешно", "Изпратихте вашето съобщение.", 2000);
            else 
                await _baseService.ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", $"{result.FailureMessage}", Request.GetDisplayUrl(), 2000);
            
            return Page();
        }

    }
}