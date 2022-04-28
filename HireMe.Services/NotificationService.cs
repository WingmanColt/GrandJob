using HireMe.Core.Helpers;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _notifyRepository;

        private readonly UserManager<User> _userManager;
        private readonly IAccountsService _accountsService;

        public NotificationService(UserManager<User> userManager, 
            IRepository<Notification> notifyRepository,
            IAccountsService accountsService)
        {
            _userManager = userManager;
            _notifyRepository = notifyRepository;
            _accountsService = accountsService;
        }

        public async Task<OperationResult> Create(string title, string url, DateTime start, NotifyType type, string icon, string receiverId, string senderId)
        {
            if (receiverId is null)
                return OperationResult.FailureResult("");

            var notification = new Notification
            {
                Title = title,
                Url = url,
                Date = start,
                Type = type,
                Icon = icon,
                SenderId = senderId,
                ReceiverId = receiverId
            };

            await _notifyRepository.AddAsync(notification);
            var result = await _notifyRepository.SaveChangesAsync();

            if(result.Success)
            {
                var user = receiverId.Contains("@") ? await _userManager.FindByEmailAsync(receiverId) : await _userManager.FindByIdAsync(receiverId);
                if (user is not null)
                {
                    user.ActivityReaded = false;
                    await _userManager.UpdateAsync(user);
                }
              //  await _signInManager.RefreshSignInAsync(user);
            }

            return result;
        }

        public async Task<bool> CreateForAdmins(string title, string url, DateTime start, NotifyType type, string icon, string senderId)
        {
            var admins = _accountsService.GetAllAdmins().ToList();

                foreach (var entity in admins)
                {
                  await Create(title, url, start, type, icon, entity.Id, senderId).ConfigureAwait(false);
                }

            return true;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Notification entity = await _notifyRepository.GetByIdAsync(id);
             _notifyRepository.Delete(entity);

            var result = await _notifyRepository.SaveChangesAsync();
            return result;
        }

        public IAsyncEnumerable<Notification> GetAllBy(User user)
        {
            if (user is null)
                return null;

            var entity = GetAllAsNoTracking()
                .Where(x => x.ReceiverId == user.Id)
                .OrderByDescending(x => x.Date)
                .AsAsyncEnumerable();


            return entity;
        }
        public async Task<OperationResult> RemoveAllBy(User user)
        {
            if (user is null)
                return OperationResult.FailureResult("User not found.");

            var entities = _notifyRepository.Set()
                .AsQueryable()
                .Where(x => x.ReceiverId == user.Id)
                .AsAsyncEnumerable();

            var isExist = await entities.IsEmptyAsync();
            if (!isExist)
            {
                await foreach (var item in entities)
                {
                    _notifyRepository.Delete(item);
                }
            }

            var result = await _notifyRepository.SaveChangesAsync();
            return result;
        }
        public async Task<int> GetNotificationsCount(User user)
        {
            if (user is null)
                return -1;

            int count = await GetAllAsNoTracking()
                .Where(j => j.ReceiverId == user.Id)
                .Select(x => new Notification { Id = x.Id })
                .AsQueryable()
                .CountAsync()
                .ConfigureAwait(false);

            return count;
        }

        private IQueryable<Notification> GetAllAsNoTracking()
        {
            return _notifyRepository.Set().AsNoTracking();
        }


    }
}