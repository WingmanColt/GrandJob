using HireMe.Entities;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Messenger
{
    [Authorize]
    public partial class MessengerModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IMessageService _messageService;

        public MessengerModel(UserManager<User> userManager, IMessageService messageService)
        {
            _userManager = userManager;
            _messageService = messageService;
        }

   
        [BindProperty]
        public CreateMessageInputModel Message { get; set; }

        [BindProperty]
        public Pager Pager { get; set; }

        public string ReturnUrl { get; set; }

        public string SearchString { get; set; }
        public string Sort { get; set; }

        public IAsyncEnumerable<Message> List { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string SearchString = null, string Sort = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (!user.MessagesEnable)
            {
                return RedirectToPage("/Messenger/Enable", new { Area = "Identity" });
            }

            if (user.AccountType == 0)
            {
                return RedirectToPage("/Account/Manage/Pricing", new { Area = "Identity" });
            }



            var entity = _messageService.GetAllAsNoTracking();


            if (!String.IsNullOrEmpty(SearchString))
            {
                entity = entity.Where(s => s.Title.ToLower().Contains(SearchString) || s.SenderId.ToLower().Contains(SearchString)).OrderByDescending(x => x.Id);
            }

            switch (Sort)
            {
                case "Входящи":
                     entity = entity.Where(x => (x.ReceiverId == user.Id) && (x.SenderId != user.Id) && !x.deletedFromReceiver && !x.isReport);
                    break;
                case "Важни":
                    entity = entity.Where(x => (x.ReceiverId == user.Id) && !x.deletedFromReceiver && x.isImportant && !x.isReport);
                    break;
                case "Изпратени":
                    entity = entity.Where(x => (x.SenderId == user.Id) && !x.deletedFromSender && !x.isReport);
                    break;
                case "Закачени":
                    entity = entity.Where(x => (x.ReceiverId == user.Id) && !x.deletedFromReceiver && x.isStared && !x.isReport);
                    break;
                case "Докладвани":
                    entity = entity.Where(x => (x.ReceiverId == user.Id) && x.isReport || (x.SenderId == user.Id) && x.isReport);
                    break;
                case "Кошче":
                    entity = entity.Where(x => x.ReceiverId == user.Id && x.deletedFromReceiver || x.SenderId == user.Id && x.deletedFromSender);
                    break;
                default:
                    entity = entity.Where(x => (x.ReceiverId == user.Id) && (x.SenderId != user.Id) && !x.deletedFromReceiver && !x.isReport);
                    break;
            }

            if (await entity.AnyAsync())
            {
                int count = await entity.AsQueryable().AsNoTracking().CountAsync();
                Pager = new Pager(count, currentPage);

                var result = entity
                //.OrderByDescending(x => x.CreatedOn)
                .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                .Take(Pager.PageSize);

                List = result.ToAsyncEnumerable();
            }
            else List = null;

            return Page();

        }
       
    }
}
