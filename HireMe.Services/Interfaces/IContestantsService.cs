namespace HireMe.Services.Interfaces
{
    using HireMe.Core.Helpers;
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

        IQueryable<Contestant> GetAllAsNoTracking();

        IAsyncEnumerable<Contestant> GetTop(int entitiesToShow);
        IAsyncEnumerable<Contestant> GetLast(int entitiesToShow);

        Task<int> GetAllCount();
        Task<Contestant> GetByIdAsync(int id);
        Task<ContestantViewModel> GetByIdAsyncMapped(int id);

    }
}