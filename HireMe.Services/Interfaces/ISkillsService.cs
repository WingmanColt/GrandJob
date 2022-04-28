namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Models;
    using HireMe.ViewModels.Contestants;
    using HireMe.ViewModels.Jobs;

    public interface ISkillsService
    {
        IAsyncEnumerable<T> GetAll<T>(string SkillId, bool isMapped);

        IAsyncEnumerable<T> GetAllById<T>(string SkillId, bool isMapped);

        Task<OperationResult> SeedSkills();

    }
}
