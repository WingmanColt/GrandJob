using System.Threading.Tasks;
using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using HireMe.Entities.Models.Chart;
using System.Linq;
using HireMe.Mapping.Utility;
using Microsoft.EntityFrameworkCore;

namespace HireMe.Services
{

    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository<JobStats> jobsStatsRepository;
        private readonly IDelayedTask _task;

        public StatisticsService(IRepository<JobStats> jobsStatsRepository, IDelayedTask task)
        {
            this.jobsStatsRepository = jobsStatsRepository;
            _task = task;
        }

        

        public async Task<OperationResult> Update(JobStatsInputModel viewModel)
        {
            JobStats Entity = await jobsStatsRepository.GetByIdAsync(viewModel.Id);

            if(Entity is null)
            {
                Entity = new JobStats();
                Entity.Update(viewModel);
                await jobsStatsRepository.AddAsync(Entity);
            } else {
                Entity.Update(viewModel);
                jobsStatsRepository.Update(Entity);
            }

            var result = await jobsStatsRepository.SaveChangesAsync();
            return result;
        }

        public IQueryable<JobStatsInputModel> GetAllTrackingMapped()
        {
            return jobsStatsRepository.Set().To<JobStatsInputModel>();
        }
        public IQueryable<JobStatsInputModel> GetAllAsNoTrackingMapped()
        {
            return jobsStatsRepository.Set().AsNoTracking().To<JobStatsInputModel>();
        }
        public async ValueTask<T> GetByIdAsync<T>(int id)
        {
            var ent = await jobsStatsRepository.Set().Where(p => p.EntityId == id).To<T>().FirstOrDefaultAsync();

            return ent;
        }


    }
}
