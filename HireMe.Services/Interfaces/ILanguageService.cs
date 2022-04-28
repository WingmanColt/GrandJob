namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Models;

    public interface ILanguageService
    {
        IAsyncEnumerable<T> GetAll<T>(string LanguageId, bool isMapped);
        IAsyncEnumerable<T> GetAllById<T>(string LanguageId, bool isMapped);
        Task<OperationResult> SeedLanguages();

    }
}
