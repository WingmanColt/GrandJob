namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Models;

    public interface ILanguageService
    {
        IAsyncEnumerable<Language> GetAll(string LanguageId);
        Task<OperationResult> SeedLanguages();

    }
}
