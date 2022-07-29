using HireMe.Core.Helpers;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IRepository<Resume> _resumeRepository;

        public ResumeService(IRepository<Resume> resumeRepository)
        {
            _resumeRepository = resumeRepository;
        }

        public async Task<OperationResult> Create(string title, string jobTitle, string fileid, int jobId, User user)
        {
           /* if (await IsResumeExists(user.Email, jobTitle, title))
            {
                return OperationResult.FailureResult("Вече съществува такъв файл !");
            }*/

            var resume = new Resume
            {
                Title = title,
                FileId = fileid,
                Date = DateTime.Now,
                UserId = user.Email,
                LastAppliedJob = jobTitle,
                JobId = jobId,  
                ResumeType = Entities.Enums.ResumeType.Active,
                isGuest = false
            };

            await _resumeRepository.AddAsync(resume);
            var result = await _resumeRepository.SaveChangesAsync();
            return result;
        }
        public async Task<int> CreateFast(string title, string fileid, int jobId, string lastAppliedJob, User user)
        {
            if (await IsResumeExists(user.Email, jobId, title))
            {
                return -2;
            }

            var resume = new Resume
            {
                Title = title,
                FileId = fileid,
                Date = DateTime.Now,
                UserId = user.Email,
                LastAppliedJob = lastAppliedJob,
                JobId = jobId,
                ResumeType = Entities.Enums.ResumeType.Active,
                isGuest = false
            };

            await _resumeRepository.AddAsync(resume);
            await _resumeRepository.SaveChangesAsync();
            return resume.Id;
        }
        public async Task<int> CreateAsGuest(string title, string fileid, string email, int jobId, string jobTitle)
        {
            if (await IsFileExists(email))
            {
                return -1;
            }

            var resume = new Resume
            {
                Title = title,
                FileId = fileid,
                LastAppliedJob = jobTitle,
                Date = DateTime.Now,
                UserId = email/*StringHelper.Filter(email)*/,
                JobId = jobId,
                ResumeType = Entities.Enums.ResumeType.Active,
                isGuest = true
            };

            await _resumeRepository.AddAsync(resume);
            await _resumeRepository.SaveChangesAsync();

            return resume.Id;
        }


        public async Task<OperationResult> Update(int id, string lastAppliedJob)
        {
            Resume existEntity = await _resumeRepository.GetByIdAsync(id);

            existEntity.LastAppliedJob = lastAppliedJob;
            _resumeRepository.Update(existEntity);

            var result = await _resumeRepository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> Delete(int id)
        {
            Resume existEntity = await _resumeRepository.GetByIdAsync(id);

            _resumeRepository.Delete(existEntity);

            var result = await _resumeRepository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> Archive(Resume entity)
        {
            //_resumeRepository.Delete(entity);

            entity.ResumeType = Entities.Enums.ResumeType.Archived;

            var result = await _resumeRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> DeleteAllBy(User user)
        {
            if (user is null)
                return OperationResult.FailureResult("User not found.");

            var entity = _resumeRepository.Set()
                .Where(j => j.UserId == user.Email)
                .ToAsyncEnumerable();

            var isExist = await entity.IsEmptyAsync();

            if (!isExist)
            {
                await foreach (var item in entity)
                {
                     //Delete(item.FileId);
                    _resumeRepository.Delete(item);
                }
            }

            var result = await _resumeRepository.SaveChangesAsync();
            return result;
        }
        public IAsyncEnumerable<Resume> GetAllBy(User user)
        {
            var entity = GetAllAsNoTracking()
                .Where(x => x.UserId == user.Email)
                .OrderByDescending(x => x.Id)
                .AsAsyncEnumerable();


            return entity;
        }

        public async Task<int> GetAllCount(User user)
        {
            return await GetAllAsNoTracking()
                .Where(x => x.UserId == user.Email)
                .CountAsync();
        }
        public IAsyncEnumerable<Resume> GetAllReceived(int jobId, ResumeType type)
        {
            //if (job is null)
             //   return null;

                //string[] items = job.resumeFilesId?.Split(',');

              //  if (items == null)
               //     return null;

                var entity = GetAllAsNoTracking()
                    .Where(x => x.JobId == jobId && x.ResumeType.Equals(type))
                    //.Where(x => ((IList)items).Contains(x.Id.ToString()))                  
                    .OrderByDescending(x => x.Id)                  
                    .AsAsyncEnumerable();

                return entity;

        }
        public async Task<bool> AddRating(Resume entity,  double rating)
        {
            if (entity == null)
                return false;

            double maxRating = 5.0;

            if (rating < maxRating)
            {
                entity.RatingVotes += (int)rating;
            }

            if (entity.Rating < (double)maxRating)
            {
                entity.Rating += ((entity.Rating * entity.RatingVotes) + rating) / (entity.RatingVotes + 1);
                entity.VotedUsers += 1;
                await _resumeRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IQueryable<Resume> GetAllAsNoTracking()
        {
            return _resumeRepository.Set().AsNoTracking();
        }

        public async Task<Resume> GetByIdAsync(int id)
        {
            var ent = await _resumeRepository.Set().FirstOrDefaultAsync(p => p.Id == id);

            return ent;
        }

        private async Task<bool> IsFileExists(string email)
        {
            string checkFileId = StringHelper.Filter(email);
            var result = await _resumeRepository.Set().AsNoTracking().AnyAsync(x => x.UserId.Contains(email) && x.FileId.Contains(checkFileId));
            return result;
            
        }
        public async Task<bool> IsResumeExists(string Email, int jobId, string title)
        {
            return await _resumeRepository.Set().AsNoTracking()
                .AnyAsync(x => x.UserId.Contains(Email) && x.JobId == jobId && x.Title == title);
        }

    }
}