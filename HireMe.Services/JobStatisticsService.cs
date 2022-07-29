using System.Threading.Tasks;
using HireMe.Data.Repository.Interfaces;
using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using HireMe.Entities.Models.Chart;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HireMe.Services.Interfaces;
using System.Collections.Generic;

namespace HireMe.Services
{

    public class JobStatisticsService : IJobStatisticsService
    {
        private readonly IRepository<JobStats> StatsRepository;

        public JobStatisticsService(IRepository<JobStats> StatsRepository)
        {
            this.StatsRepository = StatsRepository;
        }



        public async Task<OperationResult> Update(JobStatsInputModel viewModel)
        {
            var Entity = await StatsRepository.GetByIdAsync(viewModel.EntityId);

            if (Entity is null)
            {
                Entity = new JobStats();
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

        public IQueryable<JobStats> GetAllAsNoTracking(string id)
        {
            return StatsRepository.Set().Where(p => p.PosterId == id).AsNoTracking();
        }
        public IAsyncEnumerable<JobStats> GetByPosterIdAsync(string id)
        {
            var ent = StatsRepository.Set().Where(p => p.PosterId == id).ToAsyncEnumerable();
            return ent;
        }
        public async Task<JobStats> GetByIdAsync(int jobId)
        {
            var ent = await StatsRepository.Set().Where(p => p.EntityId == jobId).FirstOrDefaultAsync();
            return ent;
        }

    }
}
