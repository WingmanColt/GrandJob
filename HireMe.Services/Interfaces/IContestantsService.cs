namespace HireMe.Services.Interfaces
{
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.ViewModels.Contestants;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IContestantsService
    {
        Task<OperationResult> Create(CreateContestantInputModel viewModel, User user);
        Task<OperationResult> Delete(int id);
        Task<OperationResult> DeleteAllBy(User user);
        Task<OperationResult> Update(CreateContestantInputModel viewModel, User user);
        Task<OperationResult> UpdatePromotion(PremiumPackage type, Contestant contestant);
        Task<OperationResult> RefreshDate(Contestant Contestant);
        IQueryable<Contestant> GetAllAsNoTracking();

        IAsyncEnumerable<Contestant> GetTop(int entitiesToShow);
        IAsyncEnumerable<Contestant> GetLast(int entitiesToShow);


        Task<bool> AddRating(Contestant entity, int rating);
        Task<int> GetAllCount();
        Task<int> GetCountByUser(User user);
        Task<Contestant> GetByIdAsync(int id);
        Task<ContestantViewModel> GetByIdAsyncMapped(int id);

    }
}