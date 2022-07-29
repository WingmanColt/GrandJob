using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using HireMe.Entities.Models.Chart;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services.Interfaces
{
    public interface IJobStatisticsService
    {
        IQueryable<JobStats> GetAllAsNoTracking(string id);
        Task<JobStats> GetByIdAsync(int jobId);
        IAsyncEnumerable<JobStats> GetByPosterIdAsync(string id);
        Task<OperationResult> Update(JobStatsInputModel viewModel);
    }
}