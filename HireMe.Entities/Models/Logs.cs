using HireMe.Entities.Enums;
using JetBrains.Annotations;
using System;

namespace HireMe.Entities.Models
{
    public class Logs : BaseModel
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public LogLevel Code { get; set; }
        public string ErrorPage  { get; set; }
        public string SenderId { get; set; }

    }
}
