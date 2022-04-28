using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using HireMe.ViewModels.Contestants;
using HireMe.ViewModels.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    [Authorize]
    public class MessageController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IMessageService _messageService;
        private readonly SignInManager<User> _signInManager;

        public MessageController(UserManager<User> userManager, SignInManager<User> signInManager, IMessageService messageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageService = messageService;
        }


        public async Task<IActionResult> Enabling(string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            if (!user.MessagesEnable)
                user.MessagesEnable = true;
            else
                user.MessagesEnable = false;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendContestant(string receiver, ContestantViewModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            var receiverId = await _userManager.FindByNameAsync(receiver);

            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this._messageService.Create(input.MessageInputModel.Title, input.MessageInputModel.Description, user.Id, receiverId.Id);
            return Redirect($"/identity/messenger/index");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Send(string receiver, CreateMessageInputModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            var receiverId = await _userManager.FindByNameAsync(receiver);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this._messageService.Create(input.Title, input.Description, user.Id, receiverId.Id);
            return Redirect($"/identity/messenger/index");
        }
        [HttpPost]
        public async Task<IActionResult> SendMessageByTask(CreateMessageInputModel input, string receiver)
        {
            var user = await _userManager.GetUserAsync(User);
           
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

           /* if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }*/

            await this._messageService.Create(input.Title, input.Description, user.Id, receiver);
            return Redirect($"/identity/messenger/index");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Report(string postName, JobsViewModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }
            var usersInRole = await _userManager.GetUsersInRoleAsync("Admin");
            foreach (var admin in usersInRole)
            {
                await this._messageService.CreateReport(postName, input.Message.Description, user.Id, admin.UserName);
            }

            return RedirectToAction("Index", "Jobs", new { Id = input.Id });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ReportContestant(string fullName, ContestantViewModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync("Admin");
            foreach (var admin in usersInRole)
            {
                await this._messageService.CreateReport(fullName, input.Message.Description, user.Id, admin.UserName);
            }

            return RedirectToAction("Index", "Contestants", new { Id = input.Id });
        }
        [Authorize]
        public async Task<IActionResult> ReportMessage(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var entity = await _messageService.GetByIdAsync(id);
            if (entity == null)
            {
                return Redirect($"/Identity/Messenger/Errors/NotFound");
            }
            if (!entity.deletedFromReceiver && entity.SenderId != user.Id)
            {
                await _messageService.Add_MessageState(id, MessageStates.Report, entity.isReport = true);

                foreach (var admin in _userManager.GetUsersInRoleAsync("Admin").Result)
                {
                    await this._messageService.CreateReport($"{entity.Title} (Reported)", entity.Description, user.Id, admin.Id);
                }
            }
            return Redirect($"/Identity/Messenger/index");
        }

        public async Task<IActionResult> Stared(int id, string returnUrl = "")
        {
            var entity = await _messageService.GetByIdAsync(id);
            if (entity == null)
            {
                return Redirect($"/Identity/Messenger/Errors/NotFound");
            }

            await _messageService.Add_MessageState(id, MessageStates.Stared, entity.isStared ? false : true);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return Redirect($"/identity/messenger/index");
        }

        public async Task<IActionResult> Trash(int id, MessageClient client, string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var entity = await _messageService.GetByIdAsync(id);
            if (entity == null)
            {
                return Redirect($"/Identity/Messenger/Errors/NotFound");
            }

            await _messageService.Delete(id, user.Id, client);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return Redirect($"/identity/messenger/index");
        }
        public async Task<IActionResult> Important(int id, string returnUrl = "")
        {
            var entity = await _messageService.GetByIdAsync(id);
            if (entity == null)
            {
                return Redirect($"/Identity/Messenger/Errors/NotFound");
            }

            await _messageService.Add_MessageState(id, MessageStates.Important, entity.isImportant ? false : true);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return Redirect($"/identity/messenger/index");
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _messageService.GetByIdAsync(id);
            if (entity == null)
            {
                return Redirect($"/Identity/Messenger/Errors/NotFound");
            }

            await _messageService.Add_MessageState(id, MessageStates.Read, true);
            return Redirect($"/identity/messenger/details/{id}");
        }


    }
}
