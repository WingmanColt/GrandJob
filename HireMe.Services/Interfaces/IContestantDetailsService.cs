using HireMe.Core.Helpers;
using HireMe.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services.Interfaces
{
    public interface IContestantDetailsService
    {
        Task<OperationResult> Create(ContestantDetailsInputModel viewModel, User user);
        Task<OperationResult> Update(ContestantDetailsInputModel viewModel, User user);
        Task<OperationResult> Delete(ContestantDetails entity);
        IQueryable<ContestantDetails> GetAllAsNoTracking();
        ValueTask<ContestantDetails> GetByIdAsync(int id);

        IAsyncEnumerable<ContestantDetails> GetEducationsByUser(string PosterId);
        IAsyncEnumerable<ContestantDetails> GetWorksByUser(string PosterId);
        IAsyncEnumerable<ContestantDetails> GetAwardsByUser(string PosterId);
    }
}