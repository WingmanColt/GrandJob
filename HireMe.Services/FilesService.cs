using HireMe.Core.Helpers;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public class FilesService : IFilesService
    {
        private readonly IRepository<Files> _Repository;

        private readonly int _FilesUploadLimit;

        public FilesService(IConfiguration config, IRepository<Files> Repository)
        {
            _Repository = Repository;

            _FilesUploadLimit = config.GetValue<int>("CVPaths:FilesUploadLimit");
        }

        public async Task<OperationResult> Create(string title, string LastAppliedJob, string fileid, User user)
        {
            if (await IsFilesExists(user.Email, title))
            {
                return OperationResult.FailureResult("Вече съществува такъв файл !");
            }

            if (await GetFilesByUserCount(user.Email) >= _FilesUploadLimit)
            {
                return OperationResult.FailureResult($"Достигнахте лимита за качване на файлове. Ограничението е: {_FilesUploadLimit}");
            }


            var Files = new Files
            {
                Title = title,
                FileId = fileid,
                Date = DateTime.Now,
                UserId = user.Email,
                LastAppliedJob = LastAppliedJob
            };

            await _Repository.AddAsync(Files);
            var result = await _Repository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Update(int id, string lastAppliedJob)
        {
            Files existEntity = await _Repository.GetByIdAsync(id);

            existEntity.LastAppliedJob = lastAppliedJob;
            _Repository.Update(existEntity);

            var result = await _Repository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> Delete(Files entity)
        {
            _Repository.Delete(entity);

            var result = await _Repository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> DeleteAllBy(User user)
        {
            if (user is null)
                return OperationResult.FailureResult("User not found.");

            var entity = _Repository.Set()
                .Where(j => j.UserId == user.Email)
                .ToAsyncEnumerable();

            var isExist = await entity.IsEmptyAsync();

            if (!isExist)
            {
                await foreach (var item in entity)
                {
                    //Delete(item.FileId);
                    _Repository.Delete(item);
                }
            }

            var result = await _Repository.SaveChangesAsync();
            return result;
        }
        public IAsyncEnumerable<Files> GetAllBy(User user)
        {
            var entity = GetAllAsNoTracking()
                .Where(x => x.UserId == user.Email)
                .OrderByDescending(x => x.Id)
                .AsAsyncEnumerable();


            return entity;
        }

        public async Task<bool> AddRating(Files entity, double rating)
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

                await _Repository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IQueryable<Files> GetAllAsNoTracking()
        {
            return _Repository.Set().AsNoTracking();
        }

        public async Task<Files> GetByIdAsync(int id)
        {
            var ent = await _Repository.Set().FirstOrDefaultAsync(p => p.Id == id);

            return ent;
        }
        public async Task<Files> GetByLastAppliedJob(string LastAppliedJob)
        {
            var ent = await _Repository.Set().FirstOrDefaultAsync(p => p.LastAppliedJob == LastAppliedJob);
            return ent;
        }
        private async Task<bool> IsFilesExists(string userId, string title)
        {
            var result = await _Repository.Set().AsNoTracking().AnyAsync(x => (x.UserId == userId) && (x.Title == title));
            return result;
        }

        public async Task<int> GetFilesByUserCount(string userId)
        {
            var result = await _Repository.Set().AsNoTracking().AsQueryable().Where(x => x.UserId == userId).CountAsync().ConfigureAwait(false);
            return result;
        }

/* private async Task<bool> IsFileExists(string email)
 {
     string checkFileId = StringHelper.Filter(email.GetUntilOrEmpty("@"));
     var result = await _Repository.Set().AsNoTracking().AnyAsync(x => x.UserId.Contains(email) && x.FileId.Contains(checkFileId));
     return result;

 }*/
    }
}