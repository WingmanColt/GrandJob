namespace HireMe.Services.Interfaces
{
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.ViewModels.Jobs;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IJobsService
    {
        Task<OperationResult> Create(CreateJobInputModel viewModel, User user);
        Task<OperationResult> Update(CreateJobInputModel viewModel, User user);
        Task<OperationResult> Delete(int id);
        Task<OperationResult> DeleteAllBy(int companyId, User user);
        Task<OperationResult> AddResumeFile(int jobId, string resumeId);

        IQueryable<Jobs> GetAllAsNoTracking();
        IQueryable<Jobs> GetAll();

        IAsyncEnumerable<Jobs> GetTop(int entitiesToShow);
        IAsyncEnumerable<Jobs> GetLast(int entitiesToShow);
    //    IAsyncEnumerable<JobsViewModel> GetAllByCondition(ApproveType approved, bool archived);
        IAsyncEnumerable<Jobs> GetAllByEntity(int id, bool isCompany, int entitiesToShow);

        Task<OperationResult> RemoveResumeFromReceived(string id, Jobs job);
        Task<bool> AddRatingToJobs(int jobId, double rating);
        Task<int> GetAllCountByCondition(int categoryId, int companyId, string posterId, ApproveType approve);

        Task<JobsViewModel> GetByIdAsyncMapped(int id);
        Task<Jobs> GetByIdAsync(int id);
        Task<bool> IsValid(int Id);

    }
}