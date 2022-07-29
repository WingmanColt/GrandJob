using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HireMe.Controllers
{
    [Authorize]
    public class TasksController : BaseController
    {
        private readonly ITaskService _taskService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;

        public TasksController(
            ITaskService taskService,
            INotificationService notificationService,
            UserManager<User> userManager)
        {
            _taskService = taskService;
            _notificationService = notificationService;
            _userManager = userManager;
        }


          public async Task<IActionResult> DeleteTask(int id, string receiverId)
          {
              var user = await _userManager.GetUserAsync(User);
              var receiverUser = await _userManager.FindByIdAsync(receiverId);

            if (user == null)
            {
                  return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var result = await _taskService.Delete(id);
            if (result.Success)
            {
                if (receiverUser is not null)
                {
                    await _notificationService.Create($"Задачата е премахната с потребител {receiverUser.FirstName} {receiverUser.LastName}.", "identity/tasks/index", DateTime.Now, NotifyType.Tasks, null, user.Id, null);
                    await _notificationService.Create("Задачата е премахната от админ.", "identity/tasks/index", DateTime.Now, NotifyType.Tasks, null, receiverUser.Id, user.Id);
                }
            }

               return RedirectToPage("/Identity/Tasks", new { Area = "Identity" });
        }

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Status(int id, TasksStatus T)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            var entity = await _taskService.GetByIdAsync(id);
            if (entity == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var result = await _taskService.Status(id, T);

            if (result.Success)
            {
                    switch (T)
                    {
                        case TasksStatus.Approved:
                            await _notificationService.Create($"Вашата задача е одобрена от {user.FirstName} {user.LastName}.", "identity/tasks/index", DateTime.Now, NotifyType.Information, null, entity.SenderId, user.Id).ConfigureAwait(false);
                            break;
                        case TasksStatus.Rejected:
                            await _notificationService.Create($"Вашата задача е отхвърлена от {user.FirstName} {user.LastName}.", "identity/tasks/index", DateTime.Now, NotifyType.Information, null, entity.SenderId, user.Id).ConfigureAwait(false);
                            break;
                    } 
            }

            return RedirectToPage("/Identity/Tasks", new { Area = "Identity" });
        }
        /*
        [HttpPost]
        public async Task<IActionResult> AddTask(TaskInputModel input, string receiverId, string returnUrl = "")
        {
            var user = await _userManager.GetUserAsync(User);
            var receiverUser = await _userManager.FindByIdAsync(receiverId);
            if (user == null)
            {
                return Redirect("/Identity/Account/Errors/AccessDenied");
            }

            input.SenderId = user.Id;
            input.ReceiverId = receiverId;
            input.IsComplated = ApproveType.Waiting;

            var result = await _taskService.Create(input);

            if (result.Success)
            {
                _baseService.ToastNotifyLog(user, ToastMessageState.Success, "Успешно", "добавихте задачата си", "#", 3000);
                await _notificationService.Create($"Успешно създадохте вашата задача с {receiverUser.FirstName} {receiverUser.LastName}.", "identity/tasks/index", DateTime.Now, NotifyType.Success, "flaticon-share-1", user);

                if (receiverUser is not null)
                {
                    await _notificationService.Create("Вие получихте нова задача току-що.", "identity/tasks/index", DateTime.Now, NotifyType.Information, "flaticon-share-1", receiverUser);
                }

            }
            else
                _baseService.ToastNotifyLog(user, ToastMessageState.Error, "Грешка", result.FailureMessage, "#", 6000);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        */
    }
}
