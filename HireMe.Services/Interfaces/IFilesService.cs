using HireMe.Core.Helpers;
using HireMe.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services.Interfaces
{
    public interface IFilesService
    {
        Task<bool> AddRating(Files entity, double rating);
        Task<OperationResult> Create(string title, string fileid, User user);
        Task<OperationResult> Delete(Files entity);
        Task<OperationResult> DeleteAllBy(User user);
        IQueryable<Files> GetAllAsNoTracking();
        IAsyncEnumerable<Files> GetAllBy(User user);
        Task<Files> GetByIdAsync(int id);
        Task<int> GetFilesByUserCount(string userId);
        Task<OperationResult> Update(int id, string lastAppliedJob);
    }
}