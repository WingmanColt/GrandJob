namespace HireMe.ViewModels.Favorites
{
    using HireMe.Mapping.Interface;
    using HireMe.Entities.Models;
    public class FavoritesViewModel
    {
        public Jobs Job { get; set; }
        public Contestant Contestant { get; set; }
        public Company Company { get; set; }
        public int? JobCount { get; set; }
        public bool isExisted { get; set; }
    }

}