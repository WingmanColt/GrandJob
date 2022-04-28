using System.Threading.Tasks;
using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using System.Linq;
using System.Collections.Generic;
using HireMe.Entities.Models;
using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using Microsoft.EntityFrameworkCore;
using HireMe.Entities.Enums;
using HireMe.ViewModels.Contestants;
using HireMe.Mapping.Utility;

namespace HireMe.Services
{
   
    public class ContestantsService : IContestantsService
    {
        private readonly IRepository<Contestant> contestantRepository; 
       
        public ContestantsService(IRepository<Contestant> contestantRepository)
        {
            this.contestantRepository = contestantRepository;
        }

        public async Task<OperationResult> Create(CreateContestantInputModel viewModel, User user)
        {
            var contestant = new Contestant();
            contestant.Update(viewModel, ApproveType.Waiting, user);

            await this.contestantRepository.AddAsync(contestant);

            var result = await contestantRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Contestant entityId = await contestantRepository.GetByIdAsync(id);
            
            contestantRepository.Delete(entityId);

            var result = await contestantRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> DeleteAllBy(User user)
        {
            if (user is null)
                return OperationResult.FailureResult("User not found.");

            var entity = contestantRepository.Set()
                .Where(j => j.PosterID == user.Id)
                .AsAsyncEnumerable();

            var isExist = await entity.IsEmptyAsync();

            if (!isExist)
            {
                await foreach (var item in entity)
                {
                    contestantRepository.Delete(item);
                }
            }

            var result = await contestantRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Update(CreateContestantInputModel viewModel, User user)
        {
            Contestant existEntity = await contestantRepository.GetByIdAsync(viewModel.Id);

            existEntity.Update(viewModel, ApproveType.Waiting, user);

            this.contestantRepository.Update(existEntity);

            var result = await contestantRepository.SaveChangesAsync();
            return result;
        }
        public IQueryable<Contestant> GetAllAsNoTracking()
        {
            return contestantRepository.Set().AsNoTracking();
        }     

        public async Task<int> GetAllCount()
        {
            var ent = await GetAllAsNoTracking()
                .Where(x => x.isApproved == ApproveType.Success)
                .Select(x => new {id = x.Id, isApproved = x.isApproved })
                .AsQueryable()
                .CountAsync()
                .ConfigureAwait(false);
          
            return ent;
        }

        public async Task<int> GetCountByUserId(User user)
        {
            var entity = await contestantRepository.Set()
               .Where(j => j.PosterID == user.Id)
               .CountAsync();

            return entity;
        }

        public IAsyncEnumerable<Contestant> GetTop(int entitiesToShow)
        {
            return GetAllAsNoTracking()
                   .Where(x => (x.isApproved == ApproveType.Success) && !x.isArchived && (x.profileVisiblity == 0) && ((x.Promotion > 0) || (x.Rating > 0)))
                   .OrderBy(x => x.Id)
                   .Take(entitiesToShow)
                   .AsAsyncEnumerable();
        }
        
        public IAsyncEnumerable<Contestant> GetLast(int entitiesToShow)
        {
            var ent = GetAllAsNoTracking()
            .Where(x => (x.isApproved == ApproveType.Success) && !x.isArchived && (x.profileVisiblity == 0))
            .Select(x =>  new Contestant
            { 
               Id = x.Id,
               FullName = x.FullName, 
               payRate = x.payRate, 
               SalaryType = x.SalaryType, 
               Speciality = x.Speciality,
               PosterID = x.PosterID, 
               LocationId = x.LocationId 
            })
            .Take(entitiesToShow)
            .AsAsyncEnumerable();

             return ent;
        }

        public async Task<Contestant> GetByIdAsync(int id)
        {
            var ent = await contestantRepository.Set().FirstOrDefaultAsync(j => j.Id == id);
            return ent;
        }
        public async Task<ContestantViewModel> GetByIdAsyncMapped(int id)
        {
            var ent = await contestantRepository.Set()
                .Where(p => p.Id == id)
                .To<ContestantViewModel>()
                .FirstOrDefaultAsync();

            return ent;
        }
        /*public async Task<ContestantViewModel> GetByPosterId(string id)
        {
            var ent = await contestantRepository.Set()
                .Where(p => p.PosterID == id)
                .To<ContestantViewModel>()
                .FirstOrDefaultAsync();
           
            return null;
        }
        public async Task<bool> IsValid(int Id)
        {
            var ent = await this.contestantRepository.Set().AnyAsync(x => x.Id == Id);

            return ent;
        }
        */


     
    }
}