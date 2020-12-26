using HireMe.Core.Helpers;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Enums;
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
        
        public async Task<OperationResult> Create(int id, string userid, PromotionEnum type, DateTime start, DateTime end)
        {
            if (await IsPromotionExists(userid, type))
            {
                return OperationResult.FailureResult("Promotion failure contact with support !");
            }

            var promo = new Promotion
            {
                Id = id,
                UserId = userid,
                Type = type,
                StartTime = start,
                EndTime = end
            };

            await _promotionRepository.AddAsync(promo);

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
        private async Task<bool> IsPromotionExists(string userId, PromotionEnum promotion)
        {
            return await _promotionRepository.Set().AsQueryable().AsNoTracking().AnyAsync(x => (x.UserId == userId) && (x.Type == promotion));
        }

    }
}