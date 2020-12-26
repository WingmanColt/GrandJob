namespace HireMe.ViewModels.Favorites
{
    using HireMe.Mapping.Interface;
    using HireMe.Entities.Models;
    public class FavoritesViewModel : IMapFrom<Favorites>
    {
        public int Id { get; set; }

        public string JobsId { get; set; }

        public int CompanyId { get; set; }

        public string ContestantId { get; set; }

        public string UserId { get; set; }


    }

}