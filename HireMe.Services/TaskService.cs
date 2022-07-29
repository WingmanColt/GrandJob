using HireMe.Core.Helpers;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Enums;
using HireMe.Entities.Input;
using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<Tasks> _tasksRepository;

        public TaskService(IRepository<Tasks> tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }
        
        public async Task<OperationResult> Create(TaskInputModel input)
        {
            /*if (await IsTaskExists(input.SenderId, input.ReceiverId, input.Title))
            {
                return OperationResult.FailureResult("Задачата вече съществува!");
            }*/

            var entity = new Tasks
            {
                Title = input.Title,
                Level = input.Level,
                Status = input.Status,
                Behaviour = TasksBehaviour.Idle,
                GeneratedLink = input.GeneratedLink,
                SenderId = input.SenderId,
                ReceiverId = input.ReceiverId,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                Date = DateTime.Now
            };

            await _tasksRepository.AddAsync(entity);

            var result = await _tasksRepository.SaveChangesAsync();
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            Tasks entity = await _tasksRepository.GetByIdAsync(id);
            _tasksRepository.Delete(entity);

            var result = await _tasksRepository.SaveChangesAsync();
            return result;
        }

        public IAsyncEnumerable<Tasks> GetAll(User user, bool isReceived)
        {
            if (user is null)
                return null;

            var userId = user.Id;

            var entity = GetAllAsNoTracking()
                .Where(x => isReceived ? x.ReceiverId == userId : x.SenderId == userId)
                .AsAsyncEnumerable();

            return entity;
        }

        public async Task<int> GetAllCount(User user)
        {
            var userId = user.Id;
            var entity = await GetAllAsNoTracking()
                .Where(x => x.ReceiverId == userId || x.SenderId == userId)
                .CountAsync();

            return entity;
        }

        public async Task<Tasks> GetByLinkAsync(string link)
        {
            var ent = await _tasksRepository.Set().FirstOrDefaultAsync(j => j.GeneratedLink == link);

            return ent;
        }
        public IQueryable<Tasks> GetAllAsNoTracking()
        {
            return _tasksRepository.Set().AsNoTracking();
        }
        public async Task<Tasks> GetByIdAsync(int id)
        {
            var ent = await _tasksRepository.Set().FirstOrDefaultAsync(j => j.Id == id);

            return ent;
        }

        public async Task<OperationResult> Status(int Id, TasksStatus status)
        {
         var item = await _tasksRepository.Set().FirstOrDefaultAsync(j => j.Id == Id);
         item.Status = status;

         var success = await _tasksRepository.SaveChangesAsync();
          return success;
        }


        private async Task<bool> IsTaskExists(string senderId, string receiverId, string title)
        {
            return await _tasksRepository.Set()
                .AsQueryable()
                .AsNoTracking()
                .AnyAsync(x => (x.SenderId == senderId) && (x.ReceiverId == receiverId) && (x.Title == title));
        }

    }
}