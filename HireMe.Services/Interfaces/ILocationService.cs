namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities;
    using HireMe.Entities.Models;


    public interface ILocationService
    {
        IAsyncEnumerable<SelectListModel> GetAllSelectList();
        IQueryable<Location> GetAllAsNoTracking();

        Task<OperationResult> SeedLocation();
    }
}
