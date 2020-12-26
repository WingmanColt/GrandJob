namespace HireMe.ViewModels.Notifications
{
    using HireMe.Mapping.Interface;
    using System;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;

    public class NotificationsViewModel : IMapFrom<Notification>
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public NotifyType Type { get; set; }
        public string UserId { get; set; }
        public string Icon { get; set; }
    }
}