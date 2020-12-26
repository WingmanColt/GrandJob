using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using HireMe.ViewModels;
using HireMe.ViewModels.Contestants;
using HireMe.ViewModels.Home;
using HireMe.ViewModels.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    [RequireHttps]
    public class HomeController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly ICategoriesService _categoriesService;
        private readonly IContestantsService _contestantsService;
        private readonly ICompanyService _companyService;

        public HomeController(
            IConfiguration config,
            UserManager<User> userManager,
            ICategoriesService categoriesService,
            IContestantsService contestantsService,
            ICompanyService companyService)
        {
            _config = config;
            _userManager = userManager;
            _categoriesService = categoriesService;
            _contestantsService = contestantsService;
            _companyService = companyService;
        }

        [ResponseCache(CacheProfileName = "Weekly")]
        public async Task<IActionResult> Index(IndexViewModel viewModel)
        {
            viewModel.PictureUrl = _config.GetSection("MySettings").GetSection("CompanyImageUrl").Value;

            viewModel.User = await _userManager.GetUserAsync(User);
            viewModel.ReturnUrl = Url.PageLink();

            viewModel.AllCategories = _categoriesService.GetAllSelectList();
            viewModel.LastContestants = _contestantsService.GetLast(4);
            viewModel.TopCategories = _categoriesService.GetTop(5);
            viewModel.TopCompanies = _companyService.GetTop(10);
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Jobs(JobsViewModel jobsViewModel)
        {
            string searchStr = jobsViewModel.SearchString;
            int categoryStr = jobsViewModel.CategoryId;

            string queryString = "~/jobs?" + "SearchString=" + searchStr + '&' + "CategoryId=" + categoryStr;

            return LocalRedirect(queryString);
        }
        [HttpPost]
        public async Task<IActionResult> Candidates(ContestantViewModel contestantViewModel)
        {
            string searchStr = contestantViewModel.FullName;
            int categoryStr = contestantViewModel.CategoryId;

            string queryString = "name=" + searchStr + '&' + "category=" + categoryStr;

            return Redirect("~/contestants?" + queryString);
        }

        public async Task<IActionResult> SearchContestantCategory(int categoryId)
        {
            string queryString = "~/contestants?" + "CategoryId=" + categoryId;

            return Redirect(queryString);
        }
        public async Task<IActionResult> SearchJobsCategory(int categoryId)
        {
            string queryString = "~/jobs?" + "CategoryId=" + categoryId;

            return Redirect(queryString);
        }
        public async Task<IActionResult> SearchJobsCompany(int companyId)
        {
            string queryString = "~/jobs?" + "CompanyId=" + companyId;

            return Redirect(queryString);
        }
        public async Task<IActionResult> uploadFailure(string workerName)
        {
            return View("Errors/uploadFailure");
        }
        public async Task<IActionResult> NotFound()
        {
            return View("Errors/404");
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Privacy()
        {
            return View();
        }


        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
