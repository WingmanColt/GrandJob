using HireMe.Core.Helpers;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IRepository<Resume> _resumeRepository;

        private readonly int _resumeSizeLimit;
        private readonly string _resumeFileDir;



        public ResumeService(
            IConfiguration config,
            IRepository<Resume> resumeRepository)
        {
            _resumeRepository = resumeRepository;

            _resumeSizeLimit = config.GetValue<int>("ResumeUploadLimit");
            _resumeFileDir = config.GetValue<string>("StoredFilesPath");
        }

        public async Task<OperationResult> Create(string title, string fileid, User user)
        {
            if (await IsResumeExists(user.Id, title))
            {
                return OperationResult.FailureResult("Вече съществува такъв файл, моля променете името!");
            }

            if (await GetFilesByUserCount(user.Id) >= _resumeSizeLimit)
            {
                return OperationResult.FailureResult($"Достигнахте лимита за качване на файлове. Ограничението е: {_resumeSizeLimit}");
            }


            var resume = new Resume
            {
                Title = title,
                FileId = fileid,
                Date = DateTime.Now,
                UserId = user.Id
            };

            await _resumeRepository.AddAsync(resume);
            var result = await _resumeRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Resume entity = await _resumeRepository.GetByIdAsync(id);

            Delete(entity.FileId);
            _resumeRepository.Delete(entity);

            var result = await _resumeRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> DeleteAllBy(User user)
        {
            if (user is null)
                return OperationResult.FailureResult("User not found.");

            var entity = _resumeRepository.Set()
                .Where(j => j.UserId == user.Id)
                .ToAsyncEnumerable();

            var isExist = await entity.IsEmptyAsync();

            if (!isExist)
            {
                await foreach (var item in entity)
                {
                     Delete(item.FileId);
                    _resumeRepository.Delete(item);
                }
            }

            var result = await _resumeRepository.SaveChangesAsync();
            return result;
        }
        public IAsyncEnumerable<Resume> GetAllBy(User user)
        {
            var entity = GetAllAsNoTracking()
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.Id)
                .AsAsyncEnumerable();


            return entity;
        }
        public IAsyncEnumerable<Resume> GetAllReceived(Jobs job)
        {
            if (job is null)
                return null;

                string[] items = job.resumeFilesId?.Split(',');

                if (items == null)
                    return null;

                var entity = GetAllAsNoTracking()
                    .Where(x => ((IList)items).Contains(x.Id.ToString()))                  
                    .OrderByDescending(x => x.Id)                  
                    .AsAsyncEnumerable();

                return entity;

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
        private async Task<bool> IsResumeExists(string userId, string title)
        {
            var result = await _resumeRepository.Set().AsNoTracking().AnyAsync(x => (x.UserId == userId) && (x.Title == title));
            return result;
        }

        public async Task<int> GetFilesByUserCount(string userId)
        {
            var result = await _resumeRepository.Set().AsNoTracking().AsQueryable().Where(x => x.UserId == userId).CountAsync().ConfigureAwait(false);
            return result;
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