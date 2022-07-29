using HireMe.Core.Helpers;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IRepository<Promotion> _promotionRepository;

        public PromotionService(IRepository<Promotion> promotionRepository)
        {
            this._promotionRepository = promotionRepository;
        }
        
        public async Task<OperationResult> Create(CreatePromotion viewModel, User user)
        {
            /* if (IsPromotionExists(postType, productId))
             {
                 return OperationResult.FailureResult("Promotion failure contact with support !");
             }*/

            Promotion promo = new Promotion();
            promo.Update(viewModel, user);

            await _promotionRepository.AddAsync(promo);

            var result = await _promotionRepository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> Update(Promotion existEntity, User user)
        {
            var viewModel = new CreatePromotion
            {
                AutoSuggestion = existEntity.AutoSuggestion,
                UserId = existEntity.UserId,
                EndTime = existEntity.EndTime,
                StartTime = existEntity.StartTime,
                BoostedPost = existEntity.BoostedPost,
                BoostedPostInHome = existEntity.BoostedPostInHome,
                PostType = existEntity.PostType,
                premiumPackage = existEntity.premiumPackage,
                productId = existEntity.productId,
                RefreshCount = existEntity.RefreshCount
            };

            existEntity.Update(viewModel, user);
            _promotionRepository.Update(existEntity);

            var result = await _promotionRepository.SaveChangesAsync();


            return result;
        }
        public async Task<OperationResult> Delete(int id, string userId)
        {
            Promotion entity = await _promotionRepository.GetByIdAsync(id);
            _promotionRepository.Delete(entity);

            var result = await _promotionRepository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> DeleteAllBy(User user)
        {
            if (user is null)
                return OperationResult.FailureResult("User not found.");

            var entity = _promotionRepository.Set()
                .Where(j => j.UserId == user.Id)
                .ToAsyncEnumerable();

            var isExist = await entity.IsEmptyAsync();

            if (!isExist)
            {
                await foreach (var item in entity)
                {
                    _promotionRepository.Delete(item);
                }
            }

            var result = await _promotionRepository.SaveChangesAsync();
            return result;
        }
        public bool IsPromotionExists(PostType postType, int productId)
        {
            return _promotionRepository.Set().AsQueryable().AsNoTracking().Any(x => x.PostType == postType && x.productId == productId);
        }
        public async Task<Promotion> GetPromotion(PostType postType, int productId)
        {
            return await _promotionRepository.Set().AsQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.PostType == postType && x.productId == productId);
        }
        public async Task<Promotion> GetPromotionCounter(PostType postType, int productId)
        {
            var ent = await _promotionRepository.Set().AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(x => x.PostType == postType && x.productId == productId);

            return ent;
        }
    }
}