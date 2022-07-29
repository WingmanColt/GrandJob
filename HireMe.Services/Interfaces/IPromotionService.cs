namespace HireMe.Services.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;

    public interface IPromotionService
    {
        Task<OperationResult> Create(CreatePromotion viewModel, User user);
        Task<OperationResult> Delete(int iD, string userId);
        Task<OperationResult> DeleteAllBy(User user);
        Task<OperationResult> Update(Promotion promo, User user);
        bool IsPromotionExists(PostType postType, int productId);
        Task<Promotion> GetPromotionCounter(PostType postType, int productId);
        Task<Promotion> GetPromotion(PostType postType, int productId);
    }
}
