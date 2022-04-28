using System.Threading.Tasks;
using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using HireMe.Entities.Models.Chart;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HireMe.Services
{

    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository<Stats> StatsRepository;

        public StatisticsService(IRepository<Stats> StatsRepository)
        {
            this.StatsRepository = StatsRepository;
        }



        public async Task<OperationResult> Update(StatsInputModel viewModel)
        {
            Stats Entity = await StatsRepository.GetByIdAsync(viewModel.EntityId);

            if (Entity is null)
            {
                Entity = new Stats();
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

        public IQueryable<Stats> GetAllAsNoTracking(string id)
        {
            return StatsRepository.Set().Where(p => p.PosterId == id).AsNoTracking();
        }
        public async Task<Stats> GetByIdAsync(string id)
        {
            var ent = await StatsRepository.Set().Where(p => p.PosterId == id).FirstOrDefaultAsync();
            return ent;
        }


    }
}
