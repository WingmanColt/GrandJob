using Ardalis.GuardClauses;
using HireMe.Entities.Input;
using System;

namespace HireMe.Entities.Models
{
    public class Files : BaseModel
    {
        public string Title { get; set; }
        public string FileId { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public double Rating { get; set; }
        public int RatingVotes { get; set; }
        public int VotedUsers { get; set; }
        public string LastAppliedJob { get; set; }

        public void Update(FilesInputModel viewModel)
        {
            Id = viewModel.Id;
            LastAppliedJob = viewModel.LastAppliedJob;
        }
    }
}