namespace HireMe.Areas.Identity.Pages.Companies
{
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(Roles = "Admin, Moderator")]
    public partial class AllModel : PageModel
    {
        private readonly ICompanyService _companyService;
        public AllModel(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [BindProperty]
        public Pager Pager { get; set; }
        public IAsyncEnumerable<Company> List { get; set; }
        public string Sort { get; set; }
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string Sort = null)
        {
            ReturnUrl = Url.Page("All", new { currentPage = currentPage, area = "Identity" });

            var all = _companyService.GetAllAsNoTracking()
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
                case "Чакащи":
                    all = all.Where(x => x.isApproved == ApproveType.Waiting);
                    break;
                case "Неодобрени":
                    all = all.Where(x => x.isApproved == ApproveType.Rejected);
                    break;
            }

            all = all.OrderByDescending(x => x.Date);

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


    }
}