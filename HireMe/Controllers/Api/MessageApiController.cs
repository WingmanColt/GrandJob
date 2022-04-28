using HireMe.Data;
using HireMe.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Controllers.Api
{
    public class MessageApiController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly BaseDbContext _contextBase;

        public MessageApiController(IConfiguration config, BaseDbContext contextBase)
        {
            _config = config;
            _contextBase = contextBase ?? throw new ArgumentNullException(nameof(contextBase));
        }

        [Authorize]
        [Produces("application/json")]
        public async Task<JsonResult> AutocomplateUser(string term)
        {
            var dbSet = await _contextBase.Users.AsNoTracking()
                .Where(x => x.FirstName.Contains(term) || x.LastName.Contains(term) || x.UserName.Contains(term))
                .Select(x => new
                  {
                   id = x.UserName,
                   firstname = x.FirstName,
                   lastname = x.LastName,
                   picture = x.PictureName.Contains("https://") ? x.PictureName : _config.GetSection("MySettings").GetSection("UserPicturePath").Value + x.PictureName
                })
                
                .ToListAsync();

            return Json(dbSet);
        }

        [Authorize]
        [Produces("application/json")]
        public async Task<JsonResult> SelectRecruiter(string term)
        {
            var dbSet = _contextBase.Users.AsNoTracking()
                .Where(x => x.Role == Roles.User);

            var allowedUsers = await dbSet
                .Where(x => x.FirstName.Contains(term) || x.LastName.Contains(term) || x.UserName.Contains(term) || x.Email.Contains(term))
                .Select(x => new
                {
                    id = x.UserName,
                    firstname = x.FirstName,
                    lastname = x.LastName,
                    picture = x.PictureName.Contains("https://") ? x.PictureName : _config.GetSection("MySettings").GetSection("UserPicturePath").Value + x.PictureName 
                })
                .ToListAsync();

            return Json(allowedUsers);
        }

    }
}
