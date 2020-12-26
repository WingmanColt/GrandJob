namespace HireMe.Areas.Identity.Pages.Contestant
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(Roles = "Admin, Moderator, Contestant, User")]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IContestantsService _contestantService;

        public IndexModel(UserManager<User> userManager, IContestantsService contestantService)
        {
            this.userManager = userManager;
            this._contestantService = contestantService;
        }

        [BindProperty]
        public Pager Pager { get; set; }
        public IAsyncEnumerable<Contestant> List { get; set; }
        public string Sort { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string Sort = null)
        {
            var user = await userManager.GetUserAsync(User);
            var userid = user.Id;

            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            if (user.AccountType == 0)
            {
                return RedirectToPage("/Account/Manage/Pricing", new { Area = "Identity" });
            }

            var all = _contestantService.GetAllAsNoTracking()
               .Where(x => x.PosterID == userid)
               .Select(x => new Contestant
               {
                   Id = x.Id,
                   FullName = x.FullName,
                   CreatedOn = x.CreatedOn,
                   ExpiredOn = x.ExpiredOn,
                   isArchived = x.isArchived,
                   isApproved = x.isApproved
               });

            switch (Sort)
            {
                case "Всички":               
                    break;
                case "Одобрени":
                    all = all.Where(x => x.isApproved == ApproveType.Success);
                    break;
                case "Архивирани":
                    all = all.Where(x => x.isApproved == ApproveType.Waiting);
                    break;
            }

            if (await all.AnyAsync())
            {

                int count = await all.AsNoTracking().Distinct().CountAsync();
                Pager = new Pager(count, currentPage);

                var result = all
                .OrderByDescending(x => x.CreatedOn)
                .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                .Take(Pager.PageSize);

                List = result.AsAsyncEnumerable();
            }



            return Page();
        }



    }
}