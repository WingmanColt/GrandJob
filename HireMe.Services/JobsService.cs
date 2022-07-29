using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using HireMe.Entities.Models;
using HireMe.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using HireMe.Entities.Input;
using HireMe.Entities.Enums;
using HireMe.ViewModels.Jobs;
using HireMe.Mapping.Utility;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Collections;
using HireMe.Entities;
using System;

namespace HireMe.Services
{
    public class JobsService : IJobsService
    {
        private readonly IRepository<Jobs> jobsRepository;
        private readonly ICategoriesService _categoriesService;
        private readonly IResumeService resumeService;
        private readonly string _AppliedPath;

        public JobsService(
            IConfiguration config, 
            IRepository<Jobs> jobsRepository,
            IResumeService resumeService,
            ICategoriesService categoriesService)
        {
            this.jobsRepository = jobsRepository;
            this.resumeService = resumeService;

            _AppliedPath = config.GetValue<string>("CVPaths:AppliedCVPath");
            _categoriesService = categoriesService;
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
                    await _categoriesService.Update(item.CategoryId, true, CategoriesEnum.Decrement).ConfigureAwait(true);
                    jobsRepository.Delete(item);
                    DeleteResumeFiles(item.Name);
                }
            }

            var result = await jobsRepository.SaveChangesAsync();
            return result;
        }


        public IQueryable<Jobs> GetAllAsNoTracking()
        {
            return jobsRepository.Set().AsQueryable().AsNoTracking();
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
                PremiumPackage= x.PremiumPackage,
                Rating = x.Rating
                //isInFavourites = user != null ? _fav.isInFavourite(user, PostType.Job, x.Id.ToString()) : false
            }); 

            if (ent.Any(x => !x.Promotion.Equals(PackageType.None)))
            {
                ent = ent.OrderByDescending(x => x.Promotion);
            }
            else if (ent.Any(x => !x.PremiumPackage.Equals(PremiumPackage.None)))
            {
                ent = ent.OrderByDescending(x => x.PremiumPackage);
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
                    PremiumPackage= x.PremiumPackage,
                    CreatedOn = x.CreatedOn
                    //isInFavourites = user != null ? _fav.isInFavourite(user, PostType.Job, x.Id.ToString()) : false

                })
                .OrderByDescending(x => x.Id)
                .Take(entitiesToShow)
                .To<JobsViewModel>()
                .AsAsyncEnumerable(); 

            return result;
        }

        public OperationResult DeleteResumeFiles(string jobTitle)
        {
            string folderClearedName = Path.Combine(_AppliedPath, StringHelper.FilterTrimSplit(jobTitle));
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
                return OperationResult.SuccessResult("");
            }

            return OperationResult.FailureResult("Директорията не съществува!");
        }

        public async Task<IAsyncEnumerable<Resume>> GetAllReceivedResumes(User user, ResumeType type)
        {
            var jobs = await GetAllAsNoTracking()
            .Where(x => x.PosterID == user.Id)
            .AsQueryable()
            .ToListAsync();

            var list = new List<Resume>();
            IAsyncEnumerable<Resume> resumes;

            foreach (var item in jobs)
            {
                resumes = resumeService.GetAllReceived(item.Id, type);

                await foreach (var res in resumes)
                {
                    list.Add(res);
                }

            }

            return list.ToAsyncEnumerable();
        }
    }
}
