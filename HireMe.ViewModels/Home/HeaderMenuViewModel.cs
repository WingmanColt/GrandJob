namespace HireMe.ViewModels.Home
{
using HireMe.Entities;
using HireMe.Entities.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class HeaderMenuViewModel
    { 
        public User User { get; set; }
        public string SiteImageUrl { get; set; }
        public string Url { get; set; }
        public string FullImageCompany { get; set; }
        public string UserImageUrl { get; set; }
        public string CompanyImageUrl { get; set; }

        // Notify
        public IAsyncEnumerable<Notification> Notifications { get; set; }
        public int NotifyCount { get; set; }
        public bool isNotiftEmpty { get; set; }


        // Favourites
        public IAsyncEnumerable<Company> FavouriteCompany { get; set; }
        public IAsyncEnumerable<Jobs> FavouriteJob { get; set; }
        public IAsyncEnumerable<Contestant> FavouriteContestant { get; set; }
        public int? FavouritesCount { get; set; }

        public bool isDashboard { get; set; }
        public string ReturnUrl { get; set; }

    }
}