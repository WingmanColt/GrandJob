namespace HireMe.Services.Interfaces
{
    using HireMe.Core.Helpers;
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        IQueryable<Category> GetAllAsNoTracking();
        Task<OperationResult> Update(int categoryId, bool isJobOrCandidate, CategoriesEnum categories);
        IAsyncEnumerable<Category> GetTop(int entitiesToShow);
        IAsyncEnumerable<SelectListModel> GetAllSelectList();

        Task<OperationResult> SeedCategories();
        Task<Category> GetByIdAsync(int id);

        Task<string> GetNameById(int categoryId);
    }
}
