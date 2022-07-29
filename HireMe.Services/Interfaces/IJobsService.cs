namespace HireMe.Services.Interfaces
{
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.ViewModels.Jobs;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IJobsService
    {

        Task<OperationResult> DeleteAllBy(int companyId, User user);
        Task<IAsyncEnumerable<Resume>> GetAllReceivedResumes(User user, ResumeType type);

        IQueryable<Jobs> GetAllAsNoTracking();
        IAsyncEnumerable<JobsViewModel> GetTop(int entitiesToShow, IFavoritesService fav, User user);
        IAsyncEnumerable<JobsViewModel> GetLast(int entitiesToShow, IFavoritesService fav, User user);

    }
}