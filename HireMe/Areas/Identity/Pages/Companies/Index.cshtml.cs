namespace HireMe.Areas.Identity.Pages.Companies
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
        private readonly UserManager<User> userManager;
        private readonly ICompanyService _companyService;

        public IndexModel(UserManager<User> userManager, ICompanyService companyService)
        {
            this.userManager = userManager;
            this._companyService = companyService;
        }

        [BindProperty]
        public Pager Pager { get; set; }

        public IAsyncEnumerable<Company> List { get; set; }
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

            var all = _companyService.GetAllAsNoTracking()
                .Where(x => x.PosterId == userid || x.Admin1_Id == userid || x.Admin2_Id == userid || x.Admin3_Id == userid)
                .Select(x => new Company
                {
                  Id = x.Id,
                  Title = x.Title,
                  Date = x.Date,
                  isApproved = x.isApproved
                 }); 

            switch (Sort)
            {
                case "Всички": 
                    break;
                case "Одобрени":
                    all = all.Where(x => x.isApproved == ApproveType.Success);
                    break;
                case "Изчакващи":
                    all = all.Where(x => x.isApproved == ApproveType.Waiting);
                    break;
            }

            if (await all.AnyAsync())
            {
                
                int count = await all.Distinct().AsNoTracking().CountAsync();
                Pager = new Pager(count, currentPage);

                var result = all
                .OrderByDescending(x => x.Date)
                .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                .Take(Pager.PageSize);

                List = result.AsAsyncEnumerable();
            }

            return Page();
        }



    }
}