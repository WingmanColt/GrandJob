namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.ViewModels.Company;

    public interface ICompanyService
    {
        Task<OperationResult> Create(CreateCompanyInputModel viewModel, bool authenticEIK, User user);
        Task<OperationResult> Delete(int id);
        Task<OperationResult> DeleteAllBy(User user);
        Task<OperationResult> Update(CreateCompanyInputModel viewModel, bool authenticEIK, User user);

        IQueryable<Company> GetAllAsNoTracking();

        /*      IAsyncEnumerable<CompanyViewModel> GetAllByPosterOnly(string id);*/

        IAsyncEnumerable<SelectListModel> GetAllSelectList(User user);
        IAsyncEnumerable<Company> GetAll(User user);
        IAsyncEnumerable<Company> GetTop(int entitiesToShow);
        IAsyncEnumerable<Company> GetLast(int entitiesToShow);
        IAsyncEnumerable<Company> GetAllByApprove(ApproveType approve);

        Task<int> GetCountByUser(User user);
        Task<bool> AddRatingToCompany(Company entity, int rating);
        Task<Company> GetByIdAsync(int id);
        Task<CompanyViewModel> GetByIdAsyncMapped(int id);
        Task<bool> IsValid(int Id);
    }
}
