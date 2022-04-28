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
    public class FilterApiController : BaseController
    {
        private readonly FeaturesDbContext _contextFeatures;

        public FilterApiController(FeaturesDbContext contextFeatures)
        {
            _contextFeatures = contextFeatures ?? throw new ArgumentNullException(nameof(contextFeatures));
        }

    }
}
