namespace HireMe.Entities.Models
{
    using HireMe.Entities.Enums;
    using System;

    public class Tasks : BaseModel
    {
        public string Title { get; set; }
        public string About { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Date { get; set; }
        public TasksStatus Status { get; set; }
        public TasksBehaviour Behaviour { get; set; }
        public TaskLevel Level { get; set; }
        public string GeneratedLink { get; set; }

        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

    }
   
}