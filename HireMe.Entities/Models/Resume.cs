using System;

namespace HireMe.Entities.Models
{
    public class Resume : BaseModel
    {
        public string Title { get; set; }
        public string FileId { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int RatingVotes { get; set; }
        public string LastAppliedJob { get; set; }
    }
}
