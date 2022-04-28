namespace HireMe.ViewModels.Components
{
    using HireMe.Entities.Models;
    using System.Collections.Generic;

    public class ActivityViewModel
    {
        public User User { get; set; }
        public  string FullName { get; set; }
        public string SiteUrlUsers { get; set; }
        public string SiteUrlCompanies { get; set; }
        public string ReturnUrl { get; set; }

        public Company Company { get; set; }
        public User ContestantUser { get; set; }
        public User SenderUser { get; set; }
        public User ReceiverUser { get; set; }

        // Messages
        public IAsyncEnumerable<Message> Messages { get; set; }
        public int MessagesCount { get; set; }
        public bool isMessagesEmpty { get; set; }

        // Logs
        public IAsyncEnumerable<Logs> Logs { get; set; }

        // Tasks
        public IAsyncEnumerable<Tasks> MyTasks { get; set; }
        public IAsyncEnumerable<Tasks> ReceivedTasks { get; set; }
    }
}