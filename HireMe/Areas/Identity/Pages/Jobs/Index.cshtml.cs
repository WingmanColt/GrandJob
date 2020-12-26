namespace HireMe.Areas.Identity.Pages.Jobs
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

    [Authorize(Roles = "Admin, Moderator, Employer, Recruiter")]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IJobsService _jobsService;
        public IndexModel(
            UserManager<User> userManager,
            IJobsService jobsService)
        {
            _userManager = userManager;
            _jobsService = jobsService;
        }

        [BindProperty]
        public Pager Pager { get; set; }
        public IAsyncEnumerable<Jobs> List { get; set; }
        public string Sort { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string Sort = null)
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


            var all = _jobsService.GetAllAsNoTracking()
                .Where(x => x.PosterID == user.Id)
                .Select(x => new Jobs
                 {
                                   Id = x.Id,
                                   Name = x.Name,
                                   CreatedOn = x.CreatedOn,
                                   ExpiredOn = x.ExpiredOn,
                                   isArchived = x.isArchived,
                                   isApproved = x.isApproved
                }); 
            switch (Sort)
            {
                case "Всички":
                    all = all.OrderByDescending(x => x.CreatedOn);
                    break;
                case "Одобрени":
                    all = all.Where(x => x.isApproved == ApproveType.Success);
                    break;
                case "Архивирани":
                    all = all.Where(x => x.isArchived == true);
                    break;
                default: all = all.OrderByDescending(x => x.CreatedOn);
                    break;
            }

            if (await all.AnyAsync()) 
            {
                int count = await all.AsNoTracking().Distinct().CountAsync();
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