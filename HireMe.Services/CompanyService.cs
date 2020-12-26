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
    using HireMe.Data;
    using System;
    using HireMe.Entities.Enums;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using HireMe.ViewModels.Company;
    using HireMe.Mapping.Utility;

    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> companyRepository;
        private readonly string _FileDir;
        public CompanyService(IConfiguration config, IRepository<Company> companyRepository, FeaturesDbContext _context)
        {
            this.companyRepository = companyRepository;
            _FileDir = config.GetValue<string>("StoredImagesPath");
         //   this.SeedTest(_context);
        }

        public async Task<OperationResult> Create(CreateCompanyInputModel viewModel, bool authenticEIK, User user)
        {
            Company company = new Company();
            company.Update(viewModel, ApproveType.Waiting, authenticEIK, user);

            await this.companyRepository.AddAsync(company);
            var result = await companyRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Company entityId = await companyRepository.GetByIdAsync(id);

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
                    Delete((_FileDir + item.Logo));
                    companyRepository.Delete(item);
                }
            }

            var result = await companyRepository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> Update(CreateCompanyInputModel viewModel, bool authenticEIK, User user)
        {
            var existEntity = await companyRepository.GetByIdAsync(viewModel.Id);

            existEntity.Update(viewModel, ApproveType.Waiting, authenticEIK, user);

            this.companyRepository.Update(existEntity);

            var result = await companyRepository.SaveChangesAsync();
            return result;
        }


        public IQueryable<Company> GetAllAsNoTracking()
        {
            return companyRepository.Set().AsNoTracking();
        }

        public IAsyncEnumerable<Company> GetAllByApprove(ApproveType approve)
        {
            var ent = GetAllAsNoTracking()
                .Where(x => x.isApproved == approve)
                .AsQueryable()
                .AsAsyncEnumerable();

            return ent;
        }
        public IAsyncEnumerable<Company> GetTop(int entitiesToShow)
        {
            var ent = GetAllAsNoTracking().Where(x => x.isApproved == ApproveType.Success);

         /*   if (ent.Any(x => (x.Promotion > 0)))
            {
                ent.OrderByDescending(x => x.Promotion);
            }
            else if (ent.Any(x => x.Rating > 0))
            {
                ent.OrderByDescending(x => x.Rating);
            }
            else ent.OrderByDescending(x => x.Id);
         */
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

        public void SeedTest(FeaturesDbContext dbContext)        {            var test = new List<Company>();            for (int i = 0; i < 50000; i++)            {
                test = new List<Company>                {
                new Company
                {                Title = "test Company",                isApproved =  ApproveType.Success,                PosterId = "da3bb2af-d040-4b34-aa04-9edcfe117e80",                Adress = "Младост 4",                PhoneNumber = "13123 13213",                Date = DateTime.Now,                LocationId = "София",                About = "sexsexsexsex",                Email = "supp@gmail.com",                Private = false,                isAuthentic_EIK = true                }               };
                dbContext.Company.AddRange(test);            }
           
            dbContext.SaveChanges();        }        public int RandomNumber(int min, int max)        {            Random random = new Random();            return random.Next(min, max);        }

        public async Task<bool> AddRatingToCompany(int Id, int rating)
        {
            var job = await companyRepository.Set().FirstOrDefaultAsync(j => j.Id == Id).ConfigureAwait(false);

            if (job == null)
                return false;

            int maxRating = 5;

            if (rating < maxRating)
            {
                job.RatingVotes += rating;
            }

            if (job.Rating < (double)maxRating)
            {
                job.Rating += ((job.Rating * job.RatingVotes) + rating) / (job.RatingVotes + 1);

                await companyRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }
        private bool Delete(string fullpath)
        {
            if (!File.Exists(fullpath))
            {
                return false;
            }

            try
            {
                File.Delete(fullpath);
                return true;
            }
            catch (Exception e)
            {
            }


            return false;
        }
    }
}