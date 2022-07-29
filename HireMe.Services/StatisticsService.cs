using System.Threading.Tasks;
using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using HireMe.Entities.Models.Chart;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HireMe.Entities;

namespace HireMe.Services
{

    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository<CompanyStats> StatsRepository;

        public StatisticsService(IRepository<CompanyStats> StatsRepository)
        {
            this.StatsRepository = StatsRepository;
        }



        public async Task<OperationResult> Update(CompanyStatsInputModel viewModel)
        {
            CompanyStats Entity = await StatsRepository.GetByIdAsync(viewModel.EntityId);

            if (Entity is null)
            {
                Entity = new CompanyStats();
                Entity.Update(viewModel);
                await StatsRepository.AddAsync(Entity);
            }
            else
            {
                Entity.Update(viewModel);
                StatsRepository.Update(Entity);
            }

            var result = await StatsRepository.SaveChangesAsync();
            return result;
        }

        public IQueryable<CompanyStats> GetAllAsNoTracking(string id)
        {
            return StatsRepository.Set().Where(p => p.PosterId == id).AsNoTracking();
        }
        public async Task<CompanyStats> GetByPosterIdAsync(string id)
        {
            var ent = await StatsRepository.Set().Where(p => p.PosterId == id).FirstOrDefaultAsync();
            return ent;
        }
        public async Task<CompanyStats> GetByCompanyIdAsync(int copmpanyId)
        {
            var ent = await StatsRepository.Set().Where(p => p.EntityId == copmpanyId).FirstOrDefaultAsync();
            return ent;
        }
        /*public IAsyncEnumerable<SelectListModel> GetAllSelectList()
        {
            var list = new Dictionary<int, string>();

            list.Add(1, "Преглеждания на профила");
            list.Add(2, "Преглеждания на обявите");
            list.Add(3, "Преглеждания на фирмата");

            var result = list
                    .Select(x => new SelectListModel
                    {
                        Value = x.Key.ToString(),
                        Text = x.Value
                    })
                    .ToAsyncEnumerable();

            return result;
        }*/

    }
}
