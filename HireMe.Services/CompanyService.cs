namespace HireMe.Services
{
    using HireMe.Data.Repository.Interfaces;
    using HireMe.Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HireMe.Entities.Models;
    using HireMe.Core.Helpers;
    using Microsoft.EntityFrameworkCore;
    using HireMe.Entities.Input;
    using System;
    using HireMe.Entities.Enums;
    using HireMe.ViewModels.Company;
    using HireMe.Mapping.Utility;
    using HireMe.Entities;

    public class CompanyService : ICompanyService
    {
        private readonly IBaseService baseService;
        private readonly IRepository<Company> companyRepository;

        public CompanyService(
            IRepository<Company> companyRepository,
            IBaseService baseService)
        {
            this.companyRepository = companyRepository;
            this.baseService = baseService;
        }

        public async Task<OperationResult> Create(CreateCompanyInputModel viewModel, bool authenticEIK, User user)
        {
            Company company = new Company();
            company.Update(viewModel, ApproveType.Waiting, authenticEIK, user);

            await this.companyRepository.AddAsync(company);
            var result = await companyRepository.SaveChangesAsync();

            result.Id = company.Id;
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Company entityId = await companyRepository.GetByIdAsync(id);

            baseService.DeleteCompanyResources(entityId);
            companyRepository.Delete(entityId);

            var result = await companyRepository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> DeleteAllBy(User user)
        {
            if (user is null)
                return OperationResult.FailureResult("User not found.");

            var entity = companyRepository.Set()
                .Where(j => j.PosterId == user.Id)
                .ToAsyncEnumerable();

            var isExist = await entity.IsEmptyAsync();

            if (!isExist)
            {
                await foreach (var item in entity)
                {
                    baseService.DeleteCompanyResources(item);
                    companyRepository.Delete(item);
                }
            }

            var result = await companyRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Update(CreateCompanyInputModel viewModel, bool authenticEIK, User user)
        {
            var existEntity = await companyRepository.GetByIdAsync(viewModel.Id);

            existEntity.Update(viewModel, ApproveType.Success, authenticEIK, user);

            this.companyRepository.Update(existEntity);

            var result = await companyRepository.SaveChangesAsync();
            return result;
        }


        public IQueryable<Company> GetAllAsNoTracking()
        {
            return companyRepository.Set().AsQueryable().AsNoTracking();
        }

        public IAsyncEnumerable<Company> GetAllByApprove(ApproveType approve)
        {
            var ent = GetAllAsNoTracking()
                .Where(x => x.isApproved == approve)
                .AsAsyncEnumerable();

            return ent;
        }
        public IAsyncEnumerable<Company> GetTop(int entitiesToShow)
        {
            var ent = GetAllAsNoTracking().Where(x => x.isApproved == ApproveType.Success);

            if (ent.Any(x => (x.Promotion > 0)))
            {
                ent.OrderByDescending(x => x.Promotion);
            }
            else if (ent.Any(x => x.Rating > 0))
            {
                ent.OrderByDescending(x => x.Rating);
            }
            else ent.OrderByDescending(x => x.Id);
         
            var data = ent.Take(entitiesToShow).AsAsyncEnumerable(); 
            return data;
        }

        public IAsyncEnumerable<Company> GetLast(int entitiesToShow)
        {
            return GetAllAsNoTracking()
                   .Where(x => x.isApproved == ApproveType.Success)
                   .OrderByDescending(x => x.Id)
                   .Take(entitiesToShow)
                   .AsAsyncEnumerable();
        }

        /*public IAsyncEnumerable<Company> GetAllByPosterOnly(string id)
        {
            var entity = GetAllAsNoTracking()
                .Where(x => x.PosterId == id || x.Admin1_Id == id || x.Admin2_Id == id || x.Admin3_Id == id)
                .AsAsyncEnumerable();

            return entity;
        }*/
        public IAsyncEnumerable<Company> GetAll(User user)
        {
            if (user is null)
                return null;

            var userId = user.Id;

            var entity = GetAllAsNoTracking()
                .Where(x => x.PosterId == userId || x.Admin1_Id == userId || x.Admin2_Id == userId || x.Admin3_Id == userId)
                .AsAsyncEnumerable();

            return entity;
        }

        public IAsyncEnumerable<SelectListModel> GetAllSelectList(User user)
        {
            if (user is null)
                return null;

            var userId = user.Id;

            var entity = GetAllAsNoTracking()
                .Where(x => x.PosterId == userId || x.Admin1_Id == userId || x.Admin2_Id == userId || x.Admin3_Id == userId)
                .Select(x => new SelectListModel
                {
                    Value = x.Id.ToString(),
                    Text = x.Title.ToString()
                })
                .AsAsyncEnumerable();

            return entity;
        }
        public async Task<CompanyViewModel> GetByIdAsyncMapped(int id)
        {
            var ent = await companyRepository.Set()
                .Where(p => p.Id == id)
                .To<CompanyViewModel>()
                .FirstOrDefaultAsync();

            return ent;
        }
        public async Task<Company> GetByIdAsync(int id)
        {
            var ent = await companyRepository.Set().FirstOrDefaultAsync(j => j.Id == id);
            return ent;
        }

        public async Task<bool> IsValid(int Id)
        {
            var ent = await this.companyRepository.Set().AnyAsync(x => x.Id == Id);
            return ent;
        }

   /*     public void SeedTest()        {            var test = new List<Company>();            for (int i = 0; i < 50000; i++)            {
                test = new List<Company>                {
                new Company
                {                Title = "test Company",                isApproved =  ApproveType.Success,                PosterId = "da3bb2af-d040-4b34-aa04-9edcfe117e80",                Adress = "Младост 4",                PhoneNumber = "13123 13213",                Date = DateTime.Now,                LocationId = "София",                About = "sexsexsexsex",                Email = "supp@gmail.com",                Private = false,                isAuthentic_EIK = true                }               };
                dbContext.Company.AddRange(test);            }
           
            dbContext.SaveChanges();        }*/        public int RandomNumber(int min, int max)        {            Random random = new Random();            return random.Next(min, max);        }

        public async Task<bool> AddRatingToCompany(Company entity, int rating)
        {
            if (entity == null)
                return false;

            int maxRating = 5;

            if (rating < maxRating)
            {
                entity.RatingVotes += rating;
            }

            if (entity.Rating < (double)maxRating)
            {
                entity.Rating += ((entity.Rating * entity.RatingVotes) + rating) / (entity.RatingVotes + 1);
                entity.VotedUsers += 1;

                await companyRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<int> GetCountByUser(User user)
        {
            var res = await GetAllAsNoTracking().CountAsync(x => x.PosterId == user.Id);

            return res;
        }
    }
}