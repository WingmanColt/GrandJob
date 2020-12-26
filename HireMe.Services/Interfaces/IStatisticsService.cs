using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services.Interfaces
{


    public interface IStatisticsService
    {
        Task<OperationResult> Update(JobStatsInputModel viewModel);
        IQueryable<JobStatsInputModel> GetAllTrackingMapped();
        IQueryable<JobStatsInputModel> GetAllAsNoTrackingMapped();
        ValueTask<T> GetByIdAsync<T>(int id);
    }
}