namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;

    public interface ITaskService
    {
        Task<OperationResult> Create(TaskInputModel input);
        Task<OperationResult> Delete(int iD);

        IQueryable<Tasks> GetAllAsNoTracking();

        Task<OperationResult> Status(int Id, TasksStatus status);
        Task<Tasks> GetByLinkAsync(string link);
        Task<Tasks> GetByIdAsync(int id);
        IAsyncEnumerable<Tasks> GetAll(User user, bool isReceived);
        Task<int> GetAllCount(User user);
    }
}
