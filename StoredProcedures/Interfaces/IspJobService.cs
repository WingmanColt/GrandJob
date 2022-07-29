using Dapper;
using HireMe.Core.Helpers;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.StoredProcedures.Enums;
using HireMe.StoredProcedures.Services;

namespace HireMe.StoredProcedures.Interfaces
{
   public interface IspJobService
    {
        Task<OperationResult> CRUD(object parameters, JobCrudActionEnum action, bool AutoFindParams, string skipAttribute, User user);
        Task<IAsyncEnumerable<T>> GetAll<T>(object parameters, JobGetActionEnum state, bool AutoFindParams, string skipAttribute);
        Task<T> GetByIdAsync<T>(int id);

        Task<int> GetAllCountBy(object parameters);
        Task<bool> AddRatingToJobs(object parameters);
    }
}
