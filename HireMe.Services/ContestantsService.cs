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
using System;

namespace HireMe.Services
{
   
    public class ContestantsService : IContestantsService
    {
        private readonly IRepository<Contestant> contestantRepository;
        private readonly ICategoriesService _categoriesService;

        public ContestantsService(
            IRepository<Contestant> contestantRepository, 
            ICategoriesService categoriesService)
        {
            this.contestantRepository = contestantRepository;
            _categoriesService = categoriesService;
        }

        public async Task<OperationResult> Create(CreateContestantInputModel viewModel, User user)
        {
            var contestant = new Contestant();
            contestant.Update(viewModel, ApproveType.Waiting, user);

            await this.contestantRepository.AddAsync(contestant);
            var result = await contestantRepository.SaveChangesAsync();

            result.Id = contestant.Id;
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Contestant entityId = await contestantRepository.GetByIdAsync(id);

            await _categoriesService.Update(entityId.CategoryId, false, CategoriesEnum.Decrement).ConfigureAwait(true);
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
                    await _categoriesService.Update(item.CategoryId, false, CategoriesEnum.Decrement).ConfigureAwait(true);
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

            result.Id = existEntity.Id;
            return result;
        }
        public async Task<OperationResult> UpdatePromotion(PremiumPackage type, Contestant contestant)
        {
            contestant.PremiumPackage = type;
            this.contestantRepository.Update(contestant);
            var result = await contestantRepository.SaveChangesAsync();

            return result;
        }
        public async Task<OperationResult> RefreshDate(Contestant Contestant)
        {
            Contestant.CreatedOn = DateTime.Now;
            this.contestantRepository.Update(Contestant);

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
            .Select(x => new Contestant
            {
                Id = x.Id,
                FullName = x.FullName,
                payRate = x.payRate,
                SalaryType = x.SalaryType,
                Speciality = x.Speciality,
                PosterID = x.PosterID,
                PremiumPackage = x.PremiumPackage,
                Promotion = x.Promotion,
                LocationId = x.LocationId
            });

          /*  if (ent.Any(x => !x.Promotion.Equals(PackageType.None)))
            {
                ent = ent.OrderByDescending(x => x.Promotion);
            }
            else if (ent.Any(x => !x.PremiumPackage.Equals(PremiumPackage.None)))
            {
                ent = ent.OrderByDescending(x => x.PremiumPackage);
            }
            else if (ent.Any(x => x.Rating > 0))
            {
                ent = ent.OrderByDescending(x => x.Rating);
            }
            else ent = ent.OrderByDescending(x => x.Id);
          */
            var data = ent.Take(entitiesToShow).AsAsyncEnumerable();

            return data;
        }


        public async Task<bool> AddRating(Contestant entity, int rating)
        {
            if (entity == null)
                return false;

            int maxRating = 5;

            if (rating < maxRating)
            {
                entity.RatingVotes += rating;
            }

            if (entity.Rating < (double)maxRating)
            {
                entity.Rating += ((entity.Rating * entity.RatingVotes) + rating) / (entity.RatingVotes + 1);
                entity.VotedUsers += 1;

                await contestantRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Contestant> GetByIdAsync(int id)
        {
            var ent = await contestantRepository.Set().FirstOrDefaultAsync(j => j.Id == id);
            return ent;
        }
        public async Task<int> GetCountByUser(User user)
        {
            var ent = await contestantRepository.Set().AsQueryable().CountAsync(j => j.PosterID == user.Id);

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