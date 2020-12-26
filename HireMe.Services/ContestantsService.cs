using System.Threading.Tasks;
using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using System.Linq;
using System.Collections.Generic;
using HireMe.Entities.Models;
using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using Microsoft.EntityFrameworkCore;
using HireMe.Data;
using HireMe.Entities.Enums;
using System;
using HireMe.ViewModels.Contestants;
using HireMe.Mapping.Utility;

namespace HireMe.Services
{
   
    public class ContestantsService : IContestantsService
    {
        private readonly IRepository<Contestant> contestantRepository; 
       
        public ContestantsService(IRepository<Contestant> contestantRepository, FeaturesDbContext _context)
        {
            this.contestantRepository = contestantRepository;
          //  this.SeedTest(_context);
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
               PosterID = x.PosterID, 
               LocationId = x.LocationId 
            })
            .Take(entitiesToShow)
            .ToAsyncEnumerable();

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

        public void SeedTest(FeaturesDbContext dbContext)
        {
            var test = new List<Contestant>();

            for (int i = 0; i < 50000; i++)
            {
                test = new List<Contestant>
                {
                new Contestant
                {
                FullName = "test Performace",
                About = "I am a 28-year-old professional with over 8 years of experience working in advertising agencies both.",
                Age = DateTime.Now,
                Description= "I am a 28-year-old professional with over 8 years of experience working in advertising agencies both as a graphic designer and as a content writer. I was born in Curitiba, Brazil, where I got a degree in Advertising at Universidade Positivo before moving to Toronto, Canada, where I lived for a year.",
                SalaryType = SalaryType.month,
                Experience = 2,
                Genders = Gender.Male,
                CategoryId = 2,
                userSkillsId = "1,5,1999,2222,2212,1111,2044,2043",
                profileVisiblity = 0,
                isApproved = ApproveType.Success,
                isArchived = false,
                CreatedOn = DateTime.Now,
                ExpiredOn = DateTime.Now.AddMonths(1),
                PosterID = "da3bb2af-d040-4b34-aa04-9edcfe117e80"
                }
                 
            };
                dbContext.Contestant.AddRange(test);
            }
           
            dbContext.SaveChanges();
        }

     
    }
}