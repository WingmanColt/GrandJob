using HireMe.Entities.Enums;
using System;

namespace HireMe.Entities.Models
{
    public class Promotion : BaseModel
    {
        public string UserId { get; set; }
        public PromotionEnum Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime  { get; set; }
    }
}