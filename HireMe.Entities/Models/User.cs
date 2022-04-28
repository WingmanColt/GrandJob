namespace HireMe.Entities.Models
{
    using HireMe.Entities.Enums;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Threading.Tasks;

    public class User : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        public string PictureName { get; set; }

        [PersonalData]
        public bool profileConfirmed { get; set; }
        [PersonalData]
        public bool isExternal { get; set; }
        public bool ActivityReaded { get; set; } = true;

        [PersonalData]
        public Gender Gender { get; set; }
        public Roles Role { get; set; }
        public AccountType AccountType { get; set; } = 0;
        public DateTime ActivityOn { get; set; }

        public double Balance { get; set; } = 0;
        [PersonalData]
        public Theme Theme { get; set; } = Theme.Light;

        // Favourites
        public string FavouriteJobs { get; set; } = "0";
        public string FavouriteContestants { get; set; } = "0";
        public string FavouriteCompanies { get; set; } = "0";

        // Settings
        public bool NotifyEnable { get; set; } = true;
        public bool MessagesEnable { get; set; } = true;
        public bool EmailNotifyEnable { get; set; } = true;
        public bool SignInSocialEnable { get; set; } = true;
    }
}
