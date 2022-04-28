using HireMe.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace HireMe.Entities.Models
{
    public class Notification : BaseModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public NotifyType Type { get; set; }
        public string Icon { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        [NotMapped]
        public User userSender { get; set; }
    }
}
