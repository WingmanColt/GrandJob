using HireMe.Core.Helpers;
using HireMe.Data;
using HireMe.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Controllers.Api
{
    public class UsersApiController : BaseController
    {
        private readonly BaseDbContext _contextBase;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersApiController(
            BaseDbContext contextBase, 
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _contextBase = contextBase ?? throw new ArgumentNullException(nameof(contextBase));
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        public async Task<JsonResult> isEmailAvaliable(string term)
        {
            bool isValid = await _contextBase.Users
            .AllAsync(x => x.Email != term);
    
            return Json(isValid);
        }
     /*   [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        public async Task<JsonResult> isEmailAvaliableLogin(string term)
        {
            bool isValid = _contextBase.Users
            .AsQueryable()
            .All(x => x.Email == term);

            return Json(isValid);
        }
     */
        [HttpPost]
        [Produces("application/json")]
        public async ValueTask<JsonResult> HideBubble()
        {
            var user = await _userManager.GetUserAsync(User);

                user.ActivityReaded = true;
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);

            return Json(true);
        }

        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        public async ValueTask<JsonResult> isUsernameValid(string term)
        {
            var user = await _userManager.GetUserAsync(User);
            bool isValid;
            if (user.UserName == term || user.Email == term)
            {
                isValid = true;
            }
            else
            {
                isValid = await _contextBase.Users
               .AllAsync(x => x.UserName != term && x.Email != term);
            }

            return Json(isValid);
        }


    }
}
