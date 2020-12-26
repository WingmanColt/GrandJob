namespace HireMe.ViewModels.Location
{
    using HireMe.Mapping.Interface;
    using HireMe.Entities.Models;
    public class LocationViewModel : IMapFrom<Location>
    {
        public int Id { get; set; }

        public string City { get; set; }

    }

}