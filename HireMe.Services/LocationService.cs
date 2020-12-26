namespace HireMe.Services
{
    using HireMe.Core.Helpers;
    using HireMe.Data.Repository.Interfaces;
    using HireMe.Entities;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LocationService : ILocationService
    {
        private readonly IRepository<Location> LocationRepository;

        public LocationService(IRepository<Location> LocationRepository)
        {
            this.LocationRepository = LocationRepository;
        }

        public IAsyncEnumerable<SelectListModel> GetAllSelectList()
        {
            var result = GetAllAsNoTracking()
                    .Select(x => new SelectListModel
                    {
                        Value = x.City,
                        Text = x.City
                    })
                    .ToAsyncEnumerable();

            return result;
        }

        public IQueryable<Location> GetAllAsNoTracking()
        {
            return LocationRepository.Set().AsNoTracking().OrderByDescending(x => x.Id);
        }

        public async Task<OperationResult> SeedLocation()
        {
            if (await GetAllAsNoTracking().AnyAsync())
                return OperationResult.FailureResult("Locations already exists.");

            string[] lines = await System.IO.File.ReadAllLinesAsync(@"wwwroot/Location.txt");

            foreach (string line in lines.OrderByDescending(x => x))
            {
                var location = new Location
                {
                    City = line          
                };
                await LocationRepository.AddRangeAsync(location);
            }

           var result = await LocationRepository.SaveChangesAsync();
            return result;
        }

    }
}