using HireMe.Core.Helpers;
using HireMe.Entities;
using HireMe.Entities.Input;
using HireMe.Entities.Models.Chart;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<OperationResult> Update(CompanyStatsInputModel viewModel);
        IQueryable<CompanyStats> GetAllAsNoTracking(string id);
        Task<CompanyStats> GetByPosterIdAsync(string id);
        Task<CompanyStats> GetByCompanyIdAsync(int copmpanyId);
    }
}