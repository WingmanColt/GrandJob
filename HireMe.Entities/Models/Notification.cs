using HireMe.Entities.Enums;
using System;

namespace HireMe.Entities.Models
{
    public class Notification : BaseModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public NotifyType Type { get; set; }
        public string Icon { get; set; }
        public string UserId { get; set; }
    }
}
