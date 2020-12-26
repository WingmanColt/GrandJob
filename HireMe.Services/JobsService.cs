using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using HireMe.Entities.Models;
using HireMe.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using HireMe.Entities.Input;
using HireMe.Data;
using HireMe.Entities.Enums;
using HireMe.ViewModels.Jobs;
using HireMe.Mapping.Utility;

namespace HireMe.Services
{

    public class JobsService : IJobsService
    {
        private readonly IRepository<Jobs> jobsRepository;

        public JobsService(IRepository<Jobs> jobsRepository, FeaturesDbContext _context)
        {
            this.jobsRepository = jobsRepository;
           // SeedTest(_context);
        }

        public async Task<OperationResult> Create(CreateJobInputModel viewModel, User user)
        {
            Jobs jobs = new Jobs();
            jobs.Update(viewModel, ApproveType.Waiting, user);

            await jobsRepository.AddAsync(jobs);
            var result = await jobsRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> AddResumeFile(int jobId, string resumeId)
        {
            Jobs entityId = await jobsRepository.GetByIdAsync(jobId);
            if (entityId == null)
            {
                return OperationResult.FailureResult($"Не можахме да намерим тази обява в системата ни! Моля опитайте по-късно.");
            }
            string postIdComplate;
            
            postIdComplate = ',' + resumeId;

            entityId.resumeFilesId += postIdComplate;
            jobsRepository.Update(entityId);

            var result = await jobsRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Jobs entityId = await jobsRepository.GetByIdAsync(id);
            jobsRepository.Delete(entityId);

            var result = await jobsRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Update(CreateJobInputModel viewModel, User user)
        {
           Jobs existEntity = await jobsRepository.GetByIdAsync(viewModel.Id);

            existEntity.Update(viewModel, ApproveType.Waiting, user);
            jobsRepository.Update(existEntity);

            var result = await jobsRepository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> DeleteAllBy(int companyId, User user)
        {
            var entity = this.jobsRepository.Set().AsQueryable();
            if (companyId > 0)
            {
                entity = entity.Where(j => j.CompanyId == companyId);
            }
            if (!(user is null))
            {
                entity = entity.Where(j => j.PosterID == user.Id);
            }
            var newentity = entity.AsAsyncEnumerable();

            var isExist = await newentity.IsEmptyAsync();

            if (!isExist)
            {
                await foreach (var item in newentity)
                {
                    jobsRepository.Delete(item);
                }
            }

            var result = await jobsRepository.SaveChangesAsync();
            return result;
        }
        public async Task<int> GetAllCountByCondition(int categoryId, int companyId, string posterId, ApproveType approve)
        {
            return 0; /*await GetAllAsNoTracking()
                .Where(x => categoryId != -1 ? x.CategoryId == categoryId : false)
                .Where(x => companyId != -1 ? x.CompanyId == companyId : false)
                .Where(x => posterId != null ? x.PosterID == posterId : false)
                .Where(x => x.isApproved == approve && !x.isArchived)
                .CountAsync()
                .ConfigureAwait(false);*/
        }
      /*  public IAsyncEnumerable<Jobs> GetAllByCondition(ApproveType approved, bool archived)
        {
            var ent = GetAllAsNoTracking();
            if (approved != ApproveType.None)
            {
                ent.Where(x => x.isApproved == approved);
            }
            if (approved != ApproveType.None)
            {
                ent.Where(x => x.isArchived == archived);
            }
                ent.ToAsyncEnumerable();

            return ent;
        }
      */

        public IQueryable<Jobs> GetAllAsNoTracking()
        {
            return jobsRepository.Set().AsQueryable().AsNoTracking();
        }
        public IQueryable<Jobs> GetAll()
        {
            return jobsRepository.Set().AsQueryable();
        }
        public IAsyncEnumerable<Jobs> GetAllByEntity(int id, bool isCompany, int entitiesToShow)
        {
            return GetAllAsNoTracking()
                   .Where(x => (isCompany ? x.CompanyId == id : x.CategoryId == id))
                   .Where(x => x.isApproved == ApproveType.Success && !x.isArchived)
                   .Take(entitiesToShow)
                   .AsAsyncEnumerable();
        }

        public IAsyncEnumerable<Jobs> GetTop(int entitiesToShow)
        {
            var ent = GetAllAsNoTracking()
                   .Where(x => (x.isApproved == ApproveType.Success) && !x.isArchived);

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

        public IAsyncEnumerable<Jobs> GetLast(int entitiesToShow)
        {
            return jobsRepository.Set()
                   .Where(x => x.isApproved == ApproveType.Success && !x.isArchived)
                   .OrderByDescending(x => x.Id)
                   .Take(entitiesToShow)
                   .AsAsyncEnumerable();
        }


        public async Task<JobsViewModel> GetByIdAsyncMapped(int id)
        {
            var ent = await jobsRepository.Set()
                .Where(p => p.Id == id)
                .To<JobsViewModel>()
                .FirstOrDefaultAsync();

            return ent;
        }
        public async Task<Jobs> GetByIdAsync(int id)
        {
            var ent = await jobsRepository.Set().FirstOrDefaultAsync(j => j.Id == id);

            return ent;
        }

        public async Task<bool> IsValid(int Id)
        {
            var ent = await this.jobsRepository.Set().AnyAsync(x => x.Id == Id);
            return ent;
        }
        
        public void SeedTest(FeaturesDbContext dbContext)        {            var test = new List<Jobs>();            for (int i = 0; i < 50000; i++)            {
                test = new List<Jobs>                {
                new Jobs
                {                //Id = i,                CategoryId = 12,                CompanyId = 5,                Name = "Test for backround task",                //WorkType = Entities.Enums.WorkType.Full,                ExprienceLevels = Entities.Enums.ExprienceLevels.Beginner,                Description = "testt",                LocationId = "София",                LanguageId = null,                MinSalary = 11,                MaxSalary = 111,                SalaryType = Entities.Enums.SalaryType.day,                Adress = "test",                TagsId = null,                PosterID = "da3bb2af-d040-4b34-aa04-9edcfe117e80",                CreatedOn = DateTime.Now,                ExpiredOn = DateTime.Now.AddMonths(1),
                Promotion = Entities.Enums.PromotionEnum.Default,                isApproved = ApproveType.Success,                isArchived = false                }               };                dbContext.Jobs.AddRange(test);            }           
            dbContext.SaveChanges();        }
        public async Task<bool> AddRatingToJobs(int jobId, double rating)
        {
            var job = await this.jobsRepository.Set().FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null)
                return false;

            double maxRating = 100.0;
            
            if (job.Rating < maxRating)
            {
                job.Rating += rating;
                job.RatingVotes++;

                await jobsRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // RESUME
        public async Task<OperationResult> RemoveResumeFromReceived(string id, Jobs job)
        {
            if (job == null)
                return OperationResult.FailureResult("Операцията не може да бъде изпълнена!");

            string postIdComplate = ',' + id;
            string[] items = job.resumeFilesId?.Split(',');

            if (items == null)
                return OperationResult.FailureResult("Операцията не може да бъде изпълнена!");

            if (job.resumeFilesId?.IndexOf(id) > -1)
                {
                job.resumeFilesId = job.resumeFilesId.Remove(job.resumeFilesId.Contains(',') ? job.resumeFilesId.IndexOf(postIdComplate) : job.resumeFilesId.IndexOf(id));

                jobsRepository.Update(job);

                var result = await jobsRepository.SaveChangesAsync();
                return result;
            }
            return OperationResult.FailureResult("Операцията не може да бъде изпълнена!");
        }
    }
}
