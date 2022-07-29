namespace HireMe.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;

    public interface IAccountsService
    {
        Task<bool> Register(string username, string password, string confirmPassword, string email, string firstName, string lastName, bool isemployer);

        bool Login(string username, string password, bool rememberMe);

        void Logout();

        string LatestUsernames(string orederBy);

        void PromoteUser(string userId);

        void DemoteUser(string userId);

        Task<User> GetByIdAsync(string id);
        Task<bool> CheckUserRole(string userName, Roles[] role);
        bool CheckUserRole(User user, Roles[] role);
        
        IQueryable<User> GetAllAdmins();
        IQueryable<User> GetAllAsNoTracking();

    }
}