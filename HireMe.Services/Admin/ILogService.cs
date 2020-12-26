using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.ViewModels.Logs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public interface ILogService
    {
        Task<OperationResult> Create(string title, string errorPage, LogLevel logLevel, string userName);
        Task<OperationResult> Delete(int id);
        Task<OperationResult> DeleteAll();
        IAsyncEnumerable<Logs> GetAll();
    }
}