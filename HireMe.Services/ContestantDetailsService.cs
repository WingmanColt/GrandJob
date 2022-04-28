using System.Threading.Tasks;
using HireMe.Data.Repository.Interfaces;
using HireMe.Core.Helpers;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using System.Collections.Generic;

namespace HireMe.Services
{

    public class ContestantDetailsService : IContestantDetailsService
    {
        private readonly IRepository<ContestantDetails> _Repository;
        public ContestantDetailsService(IRepository<ContestantDetails> Repository)
        {
            _Repository = Repository;
        }


        public async Task<OperationResult> Create(ContestantDetailsInputModel viewModel, User user)
        {

            var Entity = new ContestantDetails();
            Entity.Update(viewModel, user);
            await _Repository.AddAsync(Entity);

            var result = await _Repository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Update(ContestantDetailsInputModel viewModel, User user)
        {
            var Entity = await _Repository.GetByIdAsync(viewModel.Id);

            Entity.Update(viewModel, user);
            _Repository.Update(Entity);

            var result = await _Repository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> Delete(ContestantDetails entity)
        {
            if (entity is null)
                return OperationResult.FailureResult("Entity is not exists !");

            _Repository.Delete(entity);

            var result = await _Repository.SaveChangesAsync();
            return result;
        }
        public IQueryable<ContestantDetails> GetAllAsNoTracking()
        {
            return _Repository.Set().AsNoTracking();
        }

        public async ValueTask<ContestantDetails> GetByIdAsync(int id)
        {
            var ent = await _Repository.Set().Where(p => p.Id == id).FirstOrDefaultAsync();
            return ent;
        }

        public IAsyncEnumerable<ContestantDetails> GetEducationsByUser(string PosterId)
        {
            var ent = _Repository.Set().Where(p => p.PosterId == PosterId)
                .Select(x => new ContestantDetails { 
                    Id = x.Id,
                    Education = x.Education,
                    Education_Place = x.Education_Place,
                    Education_StartDate = x.Education_StartDate,
                    Education_EndDate = x.Education_EndDate
                }).AsAsyncEnumerable();

            return ent;
        }

        public IAsyncEnumerable<ContestantDetails> GetWorksByUser(string PosterId)
        {
            var ent = _Repository.Set().Where(p => p.PosterId == PosterId)
                .Select(x => new ContestantDetails
                {
                    Id = x.Id,
                    Work = x.Education,
                    Work_Place = x.Work_Place,
                    Work_StartDate = x.Work_StartDate,
                    Work_EndDate = x.Work_EndDate
                }).AsAsyncEnumerable();

            return ent;
        }

        public IAsyncEnumerable<ContestantDetails> GetAwardsByUser(string PosterId)
        {
            var ent = _Repository.Set().Where(p => p.PosterId == PosterId)
                .Select(x => new ContestantDetails
                {
                    Id = x.Id,
                    Award = x.Award,
                    Award_Description = x.Award_Description,
                    Award_StartDate = x.Award_StartDate,
                    Award_EndDate = x.Award_EndDate
                }).AsAsyncEnumerable();

            return ent;
        }

    }
}

