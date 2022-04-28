namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
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

    [Authorize(Roles = "Admin, Moderator")]
    public partial class UsersModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IAccountsService _accountService;
        private readonly string _ImagePathShow;

        public UsersModel(IConfiguration config, UserManager<User> userManager, IAccountsService accountService)
        {
            _userManager = userManager;
            _accountService = accountService;

            _ImagePathShow = config.GetValue<string>("MySettings:UserPicturePath");
        }

        [BindProperty]
        public Pager Pager { get; set; }
        public IAsyncEnumerable<User> List { get; set; }
        public string Sort { get; set; }
        public string Role { get; set; }
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string Sort = null, string Role = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }
            if (user.Role != Roles.Admin)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            ReturnUrl = Url.Page("Users", new { currentPage = currentPage, area = "Identity" });

            var all = _accountService.GetAllAsNoTracking()
                .Select(x => new User
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Role = x.Role,
                    Email = x.Email,
                    ActivityOn = x.ActivityOn,
                    profileConfirmed = x.profileConfirmed,
                    Balance = x.Balance,
                    PictureName = x.PictureName != null && x.PictureName.Contains("https://") ? x.PictureName : (_ImagePathShow + x.PictureName)
                });

            switch (Role)
            {
                case "Всички":
                    break;
                case "Администратори":
                    all = all.Where(x => x.Role == Roles.Admin);
                    break;
                case "Модератори":
                    all = all.Where(x => x.Role == Roles.Moderator);
                    break;
                case "Работодатели":
                    all = all.Where(x => x.Role == Roles.Employer);
                    break;
                case "Персонал":
                    all = all.Where(x => x.Role == Roles.Recruiter);
                    break;
                case "Кандидати":
                    all = all.Where(x => x.Role == Roles.Contestant);
                    break;
                case "Потребители":
                    all = all.Where(x => x.Role == Roles.User);
                    break; 
            }

            switch (Sort)
            {
                case "Всички":
                    break;
                case "Потвърдени":
                    all = all.Where(x => x.profileConfirmed == true);
                    break;
                case "Активност":
                    all = all.OrderByDescending(x => x.Id);
                    break;
                case "Баланс":
                    all = all.OrderByDescending(x => x.Balance);
                    break;
            }

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


    }
}