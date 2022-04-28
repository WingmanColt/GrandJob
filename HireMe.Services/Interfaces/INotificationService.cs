namespace HireMe.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;

    public interface INotificationService
    {
        Task<OperationResult> Create(string title, string url, DateTime start, NotifyType type, string icon, string receiverId, string senderId);
        Task<bool> CreateForAdmins(string title, string url, DateTime start, NotifyType type, string icon, string senderId);
        Task<OperationResult> Delete(int iD);

        IAsyncEnumerable<Notification> GetAllBy(User user);

        Task<OperationResult> RemoveAllBy(User user);
        Task<int> GetNotificationsCount(User user);
    }
}
