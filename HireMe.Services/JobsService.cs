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
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Collections;

namespace HireMe.Services
{
    public class JobsService : IJobsService
    {
        private readonly IRepository<Jobs> jobsRepository;
        private readonly IRepository<Resume> resumeRepository;
        private readonly string _ResumeGuestsPath;

        public JobsService(
            IConfiguration config, 
            IRepository<Jobs> jobsRepository,
            IRepository<Resume> resumeRepository)
        {
            this.jobsRepository = jobsRepository;
            this.resumeRepository = resumeRepository;
            _ResumeGuestsPath = config.GetValue<string>("MySettings:ResumeGuestsPath");
        }

        public async Task<OperationResult> Create(CreateJobInputModel viewModel, User user)
        {
            Jobs jobs = new Jobs();
            jobs.Update(viewModel, ApproveType.Waiting, user);

            await jobsRepository.AddAsync(jobs);
            var result = await jobsRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> AddResumeFile(Jobs entity, string resumeId)
        {
            string postIdComplate;
            
            postIdComplate = ',' + resumeId;

            entity.resumeFilesId += postIdComplate;
            entity.ApplyCount += 1; 
            jobsRepository.Update(entity);

            var result = await jobsRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Jobs entityId = await jobsRepository.GetByIdAsync(id);

            jobsRepository.Delete(entityId);
            DeleteResumeFilesByGuest(entityId.Name);

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

        public async Task<OperationResult> UpdateUser(Jobs viewModel, User user)
        {
            viewModel.PosterID = user.Id;
            jobsRepository.Update(viewModel);

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
                    DeleteResumeFilesByGuest(item.Name);
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
        public IAsyncEnumerable<JobsViewModel> GetAllByEntity(int id, bool isCompany, int entitiesToShow, IFavoritesService _fav, User user)
        {
           var result = GetAllAsNoTracking()
                   .Where(x => (isCompany ? x.CompanyId == id : x.CategoryId == id))
                   .Where(x => x.isApproved == ApproveType.Success && !x.isArchived)
                   .Select(x => new Jobs
                     {
                      Id = x.Id,
                      Name = x.Name,
                      CompanyLogo = x.CompanyLogo,
                      LocationId = x.LocationId,
                      MinSalary = x.MinSalary,
                      MaxSalary = x.MaxSalary,
                      SalaryType = x.SalaryType,
                      ExprienceLevels = x.ExprienceLevels,
                      CreatedOn = x.CreatedOn,
                      isInFavourites = user != null ? _fav.isInFavourite(user, PostType.Job, x.Id.ToString()) : false
                    })
                   .OrderByDescending(x => x.Id)
                   .Take(entitiesToShow)
                   .To<JobsViewModel>()
                   .AsAsyncEnumerable();

            return result;
        }
        public IAsyncEnumerable<Jobs> GetAllByStats(string JobIdString)
        {
            string[] jobIds = JobIdString?.Split(',');

            if (jobIds.Length < 0)
                return null;

            var result = GetAllAsNoTracking()
                   .Where(x => ((IList)jobIds).Contains(x.Id.ToString()))
                   .AsAsyncEnumerable();

            return result;
        }
        public async Task<int> GetAllCountBy(int id, bool isCompany)
        {
            return await GetAllAsNoTracking()
                   .Where(x => (isCompany ? x.CompanyId == id : x.CategoryId == id))
                   .Where(x => x.isApproved == ApproveType.Success && !x.isArchived)
                   .CountAsync().ConfigureAwait(false);
        }
        public IAsyncEnumerable<JobsViewModel> GetTop(int entitiesToShow, IFavoritesService _fav, User user)
        {

            var ent = GetAllAsNoTracking()
            .Where(x => x.isApproved == ApproveType.Success && !x.isArchived)
            .Select(x => new Jobs
            {
                Id = x.Id,
                Name = x.Name,
                CompanyLogo = x.CompanyLogo,
                LocationId = x.LocationId,
                MinSalary = x.MinSalary,
                MaxSalary = x.MaxSalary,
                SalaryType = x.SalaryType,
                ExprienceLevels = x.ExprienceLevels,
                CreatedOn = x.CreatedOn,
                Promotion = x.Promotion,
                Rating = x.Rating,
                isInFavourites = user != null ? _fav.isInFavourite(user, PostType.Job, x.Id.ToString()) : false
            }); 

            if (ent.Any(x => x.Promotion > 0))
            {
                ent = ent.OrderByDescending(x => x.Promotion);
            }
            else if (ent.Any(x => x.Rating > 0))
            {
               ent = ent.OrderByDescending(x => x.Rating);
            }
            else ent = ent.OrderByDescending(x => x.Id);

            var data = ent.Take(entitiesToShow).To<JobsViewModel>().AsAsyncEnumerable();

            return data;
        }

        public IAsyncEnumerable<JobsViewModel> GetLast(int entitiesToShow, IFavoritesService _fav, User user)
        {
            var result = GetAllAsNoTracking()
                .Where(x => x.isApproved == ApproveType.Success && !x.isArchived)
                .Select(x => new Jobs
                {
                    Id = x.Id,
                    Name = x.Name,
                    CompanyLogo = x.CompanyLogo,
                    LocationId = x.LocationId,
                    MinSalary = x.MinSalary,
                    MaxSalary = x.MaxSalary,
                    SalaryType = x.SalaryType,
                    ExprienceLevels = x.ExprienceLevels,
                    CreatedOn = x.CreatedOn,
                    isInFavourites = user != null ? _fav.isInFavourite(user, PostType.Job, x.Id.ToString()) : false

                })
                .OrderByDescending(x => x.Id)
                .Take(entitiesToShow)
                .To<JobsViewModel>()
                .AsAsyncEnumerable(); 

            return result;
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
        public async Task<Jobs> GetByTitleAsync(string title)
        {
            var ent = await jobsRepository.Set().FirstOrDefaultAsync(j => j.Name == title);

            return ent;
        }
        public async Task<bool> IsValid(int Id)
        {
            var ent = await this.jobsRepository.Set().AnyAsync(x => x.Id == Id);
            return ent;
        }
        

        public async Task<bool> AddRatingToJobs(Jobs entity, int rating)
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

                await jobsRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        // RESUME
        public async Task<OperationResult> RemoveResumeFromReceived(int id)
        {
            Jobs job = await GetByIdAsync(id);
            if (job is null)
                return OperationResult.FailureResult("Операцията не може да бъде изпълнена!");

            string id2 = id.ToString();
            string postIdComplate = ',' + id2;
            string[] items = job.resumeFilesId?.Split(',');

            if (items is null)
                return OperationResult.FailureResult("Операцията не може да бъде изпълнена!");

            if (job.resumeFilesId?.IndexOf(id2) > -1)
                {
                job.resumeFilesId = job.resumeFilesId.Remove(job.resumeFilesId.Contains(',') ? job.resumeFilesId.IndexOf(postIdComplate) : job.resumeFilesId.IndexOf(id2));

                jobsRepository.Update(job);

                var result = await jobsRepository.SaveChangesAsync();

                if(result.Success)
                {
                    var resumes = GetAllByJobId(job.Id);
                    if (resumes is null)
                        return OperationResult.FailureResult("Операцията не може да бъде изпълнена!");


                    resumeRepository.DeleteRange(resumes);
                }
                return result;
            }
            return OperationResult.FailureResult("Операцията не може да бъде изпълнена!");
        }

        public OperationResult DeleteResumeFilesByGuest(string jobTitle)
        {
            string folderClearedName = Path.Combine(_ResumeGuestsPath, StringHelper.Filter(jobTitle));
            if (Directory.Exists(folderClearedName))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(folderClearedName);
                    FileInfo[] fileInfo = di.GetFiles();
                    foreach (var item in fileInfo)
                    {
                        item.Delete();
                    }
                }
                finally
                {
                    Directory.Delete(folderClearedName);
                }
                return OperationResult.SuccessResult(null);
            }

            return OperationResult.FailureResult("Директорията не съществува!");
        }
        private IQueryable<Resume> GetAllByJobId(int jobId)
        {
            var ent = resumeRepository.Set()
                .Where(p => p.JobId == jobId)
                .AsQueryable();

            return ent;
        }
    }
}
