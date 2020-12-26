namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Models;

    public interface IResumeService
    {
        Task<OperationResult> Create(string title, string fileid, User user);
        Task<OperationResult> Delete(int iD);
        Task<OperationResult> DeleteAllBy(User user);

        IAsyncEnumerable<Resume> GetAllBy(User user);
        IAsyncEnumerable<Resume> GetAllReceived(Jobs job);

        IQueryable<Resume> GetAllAsNoTracking();

        Task<Resume> GetByIdAsync(int id);
        Task<int> GetFilesByUserCount(string userId);

    }
}
