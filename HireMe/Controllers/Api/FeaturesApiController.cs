using HireMe.Data;
using HireMe.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Controllers.Api
{
    public class FeaturesApiController : BaseController
    {
        private readonly FeaturesDbContext _contextFeatures;

        public FeaturesApiController(FeaturesDbContext contextFeatures)
        {
            _contextFeatures = contextFeatures ?? throw new ArgumentNullException(nameof(contextFeatures));
        }

        [Produces("application/json")]
        public async Task<JsonResult> GetCategories(string term)
        {
            var dbSet = _contextFeatures.Category.AsNoTracking();

            if (!String.IsNullOrEmpty(term))
            {
                dbSet = dbSet.Where(x => x.Title_BG.ToLower().Contains(term.ToLower()));
            }

            var result = await dbSet.Select(x => new { id = x.Id, text = x.Title_BG })
                .ToListAsync();

            return Json(result);
        }


        [Produces("application/json")]
        public async Task<JsonResult> GetLocations(string term)
        {
            var dbSet = _contextFeatures.Location.AsNoTracking();

            if (!String.IsNullOrEmpty(term))
            {
                dbSet = dbSet.Where(x => x.City.ToLower().Contains(term.ToLower()));
            }

            var result = await dbSet.Select(x => new { id = x.Id, text = x.City })
                .ToListAsync();

            return Json(result);
        }


        [Produces("application/json")]
        public async Task<JsonResult> GetTags(string term)
        {
            var dbSet = _contextFeatures.Skills.AsNoTracking();

            if (!String.IsNullOrEmpty(term))
            {
                dbSet = dbSet.Where(x => x.Title.ToLower().Contains(term.ToLower()));
            }

            var result = await dbSet.Select(x => new { id = x.Title, text = x.Title })
                .ToListAsync();

            return Json(result);
        }
        
        [Produces("application/json")]
        public async Task<JsonResult> GetLanguages(string term)
        {
            var dbSet = _contextFeatures.Language.AsNoTracking();

            if (!String.IsNullOrEmpty(term))
            {
                dbSet = dbSet.Where(x => x.Name.ToLower().Contains(term.ToLower()));
            }

            var result = await dbSet.Take(15).Select(x => new {id = x.Id, text = x.Name})
                .ToListAsync();

            return Json(result);
        }


        [Produces("application/json")]
        public async Task<JsonResult> GetCompanies(string term)
        {
            var dbSet = _contextFeatures.Company.AsNoTracking();

            if (!String.IsNullOrEmpty(term))
            {
                dbSet = dbSet.Where(x => x.Title.ToLower().Contains(term.ToLower()));
            }

            var result = await dbSet.Take(5).Select(x => new { id = x.Id, text = x.Title })
                .ToListAsync();

            return Json(result);
        }
        
        [Produces("application/json")]
        public JsonResult GetWork()
        {
            var visiblityEnum = Enum.GetValues(typeof(WorkType))
                .Cast<WorkType>()
                .ToList();

            var result = visiblityEnum.Select(x => new
            {
                id = x.GetDisplayName(), /*(int)x,*/
                text = x.GetDisplayName()

            });

            return Json(result);
        }

    }
}
