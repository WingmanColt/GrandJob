using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using HireMe.Entities.Models.Chart;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<OperationResult> Update(StatsInputModel viewModel);
        IQueryable<Stats> GetAllAsNoTracking(string id);
        Task<Stats> GetByIdAsync(string id);
    }
}