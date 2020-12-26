namespace HireMe.Entities.Models
{
    using HireMe.Entities.Enums;

    public class Settings : BaseModel
    {
        public string userId { get; set; }
        public double Balance { get; set; } = 0;
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
