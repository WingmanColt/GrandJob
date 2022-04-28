namespace HireMe.ViewModels.Resume
{
    using HireMe.Mapping.Interface;
    using System;
    using HireMe.Entities.Models;

    public class ResumeViewModel : IMapFrom<Resume>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileId { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int JobId { get; set; }
        public string LastAppliedJob { get; set; }
    }
}