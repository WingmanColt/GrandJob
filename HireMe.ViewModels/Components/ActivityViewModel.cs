namespace HireMe.ViewModels.Components
{
    using HireMe.Entities.Models;
    using System.Collections.Generic;

    public class ActivityViewModel
    {
        public User User { get; set; }
        public  string FullName { get; set; }
        public string SiteUrl { get; set; }
        public string ReturnUrl { get; set; }

        public Company Company { get; set; }
        public User ContestantUser { get; set; }

        // Messages
        public IAsyncEnumerable<Message> Messages { get; set; }
        public int MessagesCount { get; set; }
        public bool isMessagesEmpty { get; set; }


        // Notify
        public IAsyncEnumerable<Notification> Notifications { get; set; }
        public int NotifyCount { get; set; }
        public bool isNotiftEmpty { get; set; }


        // Favourites
        public IAsyncEnumerable<Company> FavouriteCompany { get; set; }
        public IAsyncEnumerable<Jobs> FavouriteJob { get; set; }
        public IAsyncEnumerable<Contestant> FavouriteContestant { get; set; }
        public int? FavouritesCount { get; set; }

    }
}