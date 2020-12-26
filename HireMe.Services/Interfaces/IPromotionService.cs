namespace HireMe.Services.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;

    public interface IPromotionService
    {
        Task<OperationResult> Create(int id, string userid, PromotionEnum type, DateTime start, DateTime end);
        Task<OperationResult> Delete(int iD, string userId);
        Task<OperationResult> DeleteAllBy(User user);
    }
}
