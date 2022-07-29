namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.Services;
    using HireMe.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(Roles = "Admin")]
    public partial class LogsModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logsService;

        public LogsModel(UserManager<User> userManager, ILogService logsService)
        {
            _userManager = userManager;
            _logsService = logsService;
        }

        //[BindProperty]
        public Pager Pager { get; set; }
        public IAsyncEnumerable<Logs> List { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            var all = _logsService.GetAllAsNoTracking()
                .Select(x => new Logs
                {
                    Id = x.Id,
                    Title = x.Title,
                    Date = x.Date,
                    Code = x.Code,
                    ErrorPage = x.ErrorPage,
                    SenderId = x.SenderId
                });


            if (await all.AnyAsync())
            {
                int count = await all.AsQueryable().AsNoTracking().CountAsync();
                Pager = new Pager(count, currentPage);

                var result = all
                .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                .Take(Pager.PageSize);

                List = result.ToAsyncEnumerable();
            }
            else List = null;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            //Pager = new Pager(await List.CountAsync(), 1);
            await _logsService.DeleteAll();

            List = null;
            return Page();
        }
    }
}
