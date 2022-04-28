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
using HireMe.Entities.Models.Benchmark;

namespace HireMe.Services.Benchmark
{

    public class BenchmarkService : IBenchmarkService
    {
        private readonly IRepository<JobsTest> _jobsRepository;
        private readonly IRepository<ContestantTest> _contestantRepository;
        private readonly IRepository<CompanyTest> _companiesRepository;
        private FeaturesDbContext dbContext;

        public BenchmarkService(FeaturesDbContext _context,
              IRepository<JobsTest> jobsRepository,
              IRepository<ContestantTest> contestantRepository,
              IRepository<CompanyTest> companiesRepository)
        {
            dbContext = _context;
            _jobsRepository = jobsRepository;
            _contestantRepository = contestantRepository;
            _companiesRepository = companiesRepository;

            // SeedTest(_context);
        }

        /// Create Section
        public async Task<long> SeedJobsAsync(int count, string posterId)        {            var watch = new System.Diagnostics.Stopwatch();            var test = new List<JobsTest>();            watch.Start();            for (int i = 0; i < count - 1; i++)            {
                test = new List<JobsTest>                {
                new JobsTest
                {                //Id = i,                CategoryId = 12,                CompanyId = 1,                Name = $"Job - {i}",                //WorkType = Entities.Enums.WorkType.Full,                ExprienceLevels = Entities.Enums.ExprienceLevels.Beginner,                Description = "Lorem Ipsum",                LocationId = "София",                LanguageId = null,                MinSalary = 11,                MaxSalary = 111,                SalaryType = Entities.Enums.SalaryType.day,                Adress = "test",                TagsId = null,                PosterID = posterId,                CreatedOn = DateTime.Now,                ExpiredOn = DateTime.Now.AddMonths(1),
                Promotion = Entities.Enums.PromotionEnum.Default,                isApproved = ApproveType.Success,                isArchived = false                }               };            }

            await dbContext.AddRangeAsync(test);
            await dbContext.SaveChangesAsync();

            watch.Stop();            return watch.ElapsedMilliseconds;
        }

        public long SeedJobs(int count, string posterId)        {
            var watch = new System.Diagnostics.Stopwatch();            IList<JobsTest> test = new List<JobsTest>();

            watch.Start();            for (int i = 0; i < count - 1; i++)            {
                test = new List<JobsTest>                {
                new JobsTest
                {                //Id = i,                CategoryId = 12,                CompanyId = 5,                Name = $"Job - {i}",                //WorkType = Entities.Enums.WorkType.Full,                ExprienceLevels = Entities.Enums.ExprienceLevels.Beginner,                Description = "Lorem Ipsum",                LocationId = "София",                LanguageId = null,                MinSalary = 11,                MaxSalary = 111,                SalaryType = Entities.Enums.SalaryType.day,                Adress = "test",                TagsId = null,                PosterID = posterId,                CreatedOn = DateTime.Now,                ExpiredOn = DateTime.Now.AddMonths(1),
                Promotion = Entities.Enums.PromotionEnum.Default,                isApproved = ApproveType.Success,                isArchived = false                }               };                dbContext.JobsTest.AddRange(test);            }

            dbContext.SaveChanges();
            watch.Stop();

            return watch.ElapsedMilliseconds;        }




        public async Task<long> SeedContestantsAsync(int count, string posterId)        {            var watch = new System.Diagnostics.Stopwatch();            var test = new List<ContestantTest>();            watch.Start();            for (int i = 0; i < count - 1; i++)            {
                test = new List<ContestantTest>                {
                new ContestantTest
                {                FullName = "test Performace",
                About = "I am a 28-year-old professional with over 8 years of experience working in advertising agencies both.",
                Age = DateTime.Now,
                Description= "I am a 28-year-old professional with over 8 years of experience working in advertising agencies both as a graphic designer and as a content writer. I was born in Curitiba, Brazil, where I got a degree in Advertising at Universidade Positivo before moving to Toronto, Canada, where I lived for a year.",
                SalaryType = SalaryType.month,
                Experience = 2,
                Genders = Gender.Male,
                CategoryId = 2,
                userSkillsId = "1,5,1999,2222,2212,1111,2044,2043",
                profileVisiblity = 0,
                isApproved = ApproveType.Success,
                isArchived = false,
                CreatedOn = DateTime.Now,
                ExpiredOn = DateTime.Now.AddMonths(1),
                PosterID = posterId                }               };

            }
            await dbContext.AddRangeAsync(test);
            await dbContext.SaveChangesAsync();

            watch.Stop();            return watch.ElapsedMilliseconds;
        }

        public long SeedContestants(int count, string posterId)        {
            var watch = new System.Diagnostics.Stopwatch();            var test = new List<ContestantTest>();

            watch.Start();            for (int i = 0; i < count - 1; i++)            {
                test = new List<ContestantTest>                {
                new ContestantTest
                {                FullName = "test Performace",
                About = "I am a 28-year-old professional with over 8 years of experience working in advertising agencies both.",
                Age = DateTime.Now,
                Description= "I am a 28-year-old professional with over 8 years of experience working in advertising agencies both as a graphic designer and as a content writer. I was born in Curitiba, Brazil, where I got a degree in Advertising at Universidade Positivo before moving to Toronto, Canada, where I lived for a year.",
                SalaryType = SalaryType.month,
                Experience = 2,
                Genders = Gender.Male,
                CategoryId = 2,
                userSkillsId = "1,5,1999,2222,2212,1111,2044,2043",
                profileVisiblity = 0,
                isApproved = ApproveType.Success,
                isArchived = false,
                CreatedOn = DateTime.Now,
                ExpiredOn = DateTime.Now.AddMonths(1),
                PosterID = posterId                }               };                dbContext.ContestantTest.AddRange(test);            }

            dbContext.SaveChanges();
            watch.Stop();

            return watch.ElapsedMilliseconds;        }



        public async Task<long> SeedCompanyAsync(int count, string posterId)        {            var watch = new System.Diagnostics.Stopwatch();            var test = new List<CompanyTest>();            watch.Start();            for (int i = 0; i < count - 1; i++)            {
                test = new List<CompanyTest>                {
                new CompanyTest
                {                Title = $"Company - {i}",                isApproved =  ApproveType.Success,                PosterId = posterId,                Adress = "Младост 4",                PhoneNumber = "13123 13213",                Date = DateTime.Now,                LocationId = "София",                About = "sexsexsexsex",                Email = "supp@gmail.com",                Private = false,                isAuthentic_EIK = true                }               };            }

            await dbContext.AddRangeAsync(test);
            await dbContext.SaveChangesAsync();

            watch.Stop();            return watch.ElapsedMilliseconds;
        }

        public long SeedCompany(int count, string posterId)        {
            var watch = new System.Diagnostics.Stopwatch();            var test = new List<CompanyTest>();

            watch.Start();            for (int i = 0; i < count - 1; i++)            {
                test = new List<CompanyTest>                {
                new CompanyTest
                {                Title = $"Company - {i}",                isApproved =  ApproveType.Success,                PosterId = posterId,                Adress = "Младост 4",                PhoneNumber = "13123 13213",                Date = DateTime.Now,                LocationId = "София",                About = "sexsexsexsex",                Email = "supp@gmail.com",                Private = false,                isAuthentic_EIK = true                }               };                dbContext.CompanyTest.AddRange(test);            }

            dbContext.SaveChanges();
            watch.Stop();

            return watch.ElapsedMilliseconds;        }



        /// Delete Section
        public async Task<long> RemoveJobsAsync(User user)
        {
            var watch = new System.Diagnostics.Stopwatch();

            if (user is null)
                return -1;

            watch.Start();

            var entities = _jobsRepository.Set()
                .AsQueryable()
                .AsAsyncEnumerable();

            var isExist = await entities.IsEmptyAsync();
            if (!isExist)
            {
                await foreach (var item in entities)
                {
                    _jobsRepository.Delete(item);
                }
            }

            await _jobsRepository.SaveChangesAsync();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
        public long RemoveJobs(User user)
        {
            var watch = new System.Diagnostics.Stopwatch();

            if (user is null)
                return -1;

            watch.Start();
            var entities = _jobsRepository.Set()
                .AsQueryable()
                .ToList();

            var isExist = entities.FirstOrDefault();
            if (isExist != null)
            {
                foreach (var item in entities)
                {
                    _jobsRepository.Delete(item);
                }
            }

             _jobsRepository.SaveChanges();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }




        public async Task<long> RemoveContestantsAsync(User user)
        {
            var watch = new System.Diagnostics.Stopwatch();

            if (user is null)
                return -1;

            watch.Start();

            var entities = _contestantRepository.Set()
                .AsQueryable()
                .AsAsyncEnumerable();

            var isExist = await entities.IsEmptyAsync();
            if (!isExist)
            {
                await foreach (var item in entities)
                {
                    _contestantRepository.Delete(item);
                }
            }

            await _contestantRepository.SaveChangesAsync();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
        public long RemoveContestants(User user)
        {
            var watch = new System.Diagnostics.Stopwatch();

            if (user is null)
                return -1;

            watch.Start();
            var entities = _contestantRepository.Set()
                .AsQueryable()
                .ToList();

            var isExist = entities.FirstOrDefault();
            if (isExist != null)
            {
                foreach (var item in entities)
                {
                    _contestantRepository.Delete(item);
                }
            }

             _contestantRepository.SaveChanges();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }



        public async Task<long> RemoveCompaniesAsync(User user)
        {
            var watch = new System.Diagnostics.Stopwatch();

            if (user is null)
                return -1;

            watch.Start();
            var entities = _companiesRepository.Set()
                .AsQueryable()
                .AsAsyncEnumerable();

            var isExist = await entities.IsEmptyAsync();
            if (!isExist)
            {
                await foreach (var item in entities)
                {
                    _companiesRepository.Delete(item);
                }
            }

            await _companiesRepository.SaveChangesAsync();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
        public long RemoveCompanies(User user)
        {
            var watch = new System.Diagnostics.Stopwatch();

            if (user is null)
                return -1;

            watch.Start();
            var entities = _companiesRepository.Set()
                .AsQueryable()
                .ToList();

            var isExist = entities.FirstOrDefault();
            if (isExist != null)
            {
                foreach (var item in entities)
                {
                    _companiesRepository.Delete(item);
                }
            }

             _companiesRepository.SaveChanges();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
    }
}
