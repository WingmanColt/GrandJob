using HireMe.Services.Interfaces;
using HireMe.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService _categoriesService;
        public CategoriesController(ICategoriesService categoriesService)
        {
            this._categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CategoriesViewModel viewModel)
        {
            var entity = _categoriesService.GetAllAsNoTracking().AsAsyncEnumerable();

            viewModel.List = await entity.AnyAsync() ? entity : null;

            return View(viewModel);
        }
    }
}
