using HireMe.Core.Helpers;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using HireMe.Mapping.Utility;
using HireMe.ViewModels.Logs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public class LogService : ILogService
    {
        private readonly IRepository<Logs> logsRepository;

        public LogService(IRepository<Logs> logsRepository)
        {
            this.logsRepository = logsRepository;
        }


        public async Task<OperationResult> Create(string title, string errorPage, LogLevel logLevel, string userName)
        {
            var log = new Logs
            {
                Title = title,
                ErrorPage = errorPage,
                Code = logLevel,
                Date = DateTime.Now,
                SenderId = userName
            };

            await logsRepository.AddAsync(log);

            var result = await logsRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Logs entity = await logsRepository.GetByIdAsync(id);
            logsRepository.Delete(entity);

            var result = await logsRepository.SaveChangesAsync();
            return result;
        }

            public async Task<OperationResult> DeleteAll()
            {
            var all = logsRepository.Set().AsQueryable();

            logsRepository.DeleteRange(all);

            var result = await logsRepository.SaveChangesAsync();
            return result;
            }

            public IAsyncEnumerable<Logs> GetAll()
        {
            var entity = GetAllAsNoTracking()
                .OrderByDescending(x => x.Date)
                .ToAsyncEnumerable();

            return entity;
        }

        private IQueryable<Logs> GetAllAsNoTracking()
        {
            return logsRepository.Set().AsNoTracking();
        }
    }
}
