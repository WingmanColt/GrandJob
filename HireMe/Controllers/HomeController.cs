using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using HireMe.ViewModels;
using HireMe.ViewModels.Contestants;
using HireMe.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly ICategoriesService _categoriesService;
        private readonly IContestantsService _contestantsService;
        private readonly ICompanyService _companyService;
        private readonly IJobsService _jobsService;
        private readonly IFavoritesService _favouriteService;
        public HomeController(
            IConfiguration config,
            UserManager<User> userManager,
            ICategoriesService categoriesService,
            IContestantsService contestantsService,
            ICompanyService companyService,
            IJobsService jobsService,
            IFavoritesService favouriteService)
        {
            _config = config;
            _userManager = userManager;
            _categoriesService = categoriesService;
            _contestantsService = contestantsService;
            _companyService = companyService;
            _jobsService = jobsService;
            _favouriteService = favouriteService;
        }

        [ResponseCache(CacheProfileName = "Hourly")]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index(IndexViewModel viewModel, [FromServices] ISkillsService _skillsService, [FromServices] ILocationService _locationService)
        {
            var user = await _userManager.GetUserAsync(User);

            viewModel.PictureUrl = _config.GetSection("MySettings").GetSection("CompanyImageUrl").Value;
            viewModel.UserPicturePath = _config.GetSection("MySettings").GetSection("UserPicturePath").Value;
            viewModel.SiteUrl = _config.GetSection("MySettings").GetSection("SiteUrl").Value;


            viewModel.User = await _userManager.GetUserAsync(User);
            viewModel.ReturnUrl = Url.PageLink();

            viewModel.AllLocations = _locationService.GetAllSelectList();
            viewModel.AllCategories = _categoriesService.GetAllSelectList();
            viewModel.LastContestants = _contestantsService.GetLast(4);
            viewModel.TopCategories = _categoriesService.GetTop(8);
            viewModel.TopCompanies = _companyService.GetTop(10);

            viewModel.JobsLast = _jobsService.GetLast(6, _favouriteService, user);
            viewModel.JobsTop = _jobsService.GetTop(6, _favouriteService, user);
           // viewModel.JobsByCompany = _jobsService.GetAllByEntity(1, true, 6);

           // viewModel.Skills = _skillsService.GetAll<Skills>(viewModel.SearchString, false);

            return View(viewModel);
        }

        [Authorize]
        public async Task<ActionResult> RemoveFavourites(string returnUrl, [FromServices] IFavoritesService _favoriteService)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            await _favoriteService.RemoveAllFavourites(user).ConfigureAwait(false);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        
        public IActionResult uploadFailure(string workerName)
        {
            return View("Errors/uploadFailure");
        }
        public IActionResult NotFound()
        {
            return View("Errors/404");
        }
        

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult Contacts()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult faq()
        {
            return View();
        }
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
