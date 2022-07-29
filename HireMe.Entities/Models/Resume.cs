using Ardalis.GuardClauses;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using System;

namespace HireMe.Entities.Models
{
    public class Resume : BaseModel
    {
        public string Title { get; set; }
        public string FileId { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public double Rating { get; set; }
        public int RatingVotes { get; set; }
        public int VotedUsers { get; set; }
        public string LastAppliedJob { get; set; }
        public int JobId { get; set; }
        public bool isGuest { get; set; }
        public ResumeType ResumeType { get; set; }
        public void Update(CreateResumeInputModel viewModel)
        {
            Id = viewModel.Id;
            LastAppliedJob = viewModel.LastAppliedJob;
        }
    }
}