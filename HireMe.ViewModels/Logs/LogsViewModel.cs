namespace HireMe.ViewModels.Logs
{
    using HireMe.Mapping.Interface;
    using System;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;

    public class LogsViewModel : IMapFrom<Logs>
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string ErrorPage { get; set; }
        public DateTime Date { get; set; }
        public LogLevel Code { get; set; }
        public string SenderId { get; set; }
    }
}