using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Input
{

    public class CreateResumeInputModel : BaseModel
    {
        public string Title { get; set; }
        public string FileId { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int RatingVotes { get; set; }
        public string LastAppliedJob { get; set; }
        public int JobId { get; set; }
        public IFormFile FormFile { get; set; }

    }
}
