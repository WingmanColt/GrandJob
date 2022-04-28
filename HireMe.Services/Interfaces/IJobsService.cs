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
        Task<OperationResult> UpdateUser(Jobs viewModel, User user);
        Task<OperationResult> Delete(int id);
        Task<OperationResult> DeleteAllBy(int companyId, User user);
        Task<OperationResult> AddResumeFile(Jobs job, string resumeId);

        IQueryable<Jobs> GetAllAsNoTracking();
        IQueryable<Jobs> GetAll();

        IAsyncEnumerable<JobsViewModel> GetTop(int entitiesToShow, IFavoritesService fav, User user);
        IAsyncEnumerable<JobsViewModel> GetLast(int entitiesToShow, IFavoritesService fav, User user);
    //    IAsyncEnumerable<JobsViewModel> GetAllByCondition(ApproveType approved, bool archived);
        IAsyncEnumerable<JobsViewModel> GetAllByEntity(int id, bool isCompany, int entitiesToShow, IFavoritesService _fav, User user);
        IAsyncEnumerable<Jobs> GetAllByStats(string JobIdString);
        Task<OperationResult> RemoveResumeFromReceived(int id);
        Task<bool> AddRatingToJobs(Jobs entity, int rating);
        Task<int> GetAllCountByCondition(int categoryId, int companyId, string posterId, ApproveType approve);
        Task<int> GetAllCountBy(int id, bool isCompany);

        Task<JobsViewModel> GetByIdAsyncMapped(int id);
        Task<Jobs> GetByIdAsync(int id);
        Task<bool> IsValid(int Id);

    }
}