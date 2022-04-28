namespace HireMe.ViewModels.Notifications
{
    using HireMe.Mapping.Interface;
    using System;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations.Schema;

    public class NotificationsViewModel : IMapFrom<Notification>
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public NotifyType Type { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Icon { get; set; }
        public Task<User> userSender { get; set; }
    }
}