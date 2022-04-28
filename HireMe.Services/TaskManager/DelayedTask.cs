namespace HireMe.Services
{
    using HireMe.Core.Extensions;
    using HireMe.Core.Helpers;
    using HireMe.Data.Repository.Interfaces;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class DelayedTask : IDelayedTask
    {
        private readonly ILogService logger;
        private readonly IRepository<Jobs> _jobsRepository;
        private readonly IRepository<Contestant> _contestantRepository;
        private readonly IRepository<Tasks> _tasksRepository;
        private readonly string _WebUrl;

        public DelayedTask(
            IConfiguration config,
            ILogService logger, 
            IRepository<Jobs> jobsRepository,
            IRepository<Contestant> contestantRepository,
            IRepository<Tasks> tasksRepository)
        {
            this.logger = logger;
            _jobsRepository = jobsRepository;
            _contestantRepository = contestantRepository;
            _tasksRepository = tasksRepository;

            _WebUrl = config.GetValue<string>("MySettings:SiteWebCamUrl");
        }

        public async Task DoWork(CancellationToken token)
        {

            // while (true)
            // {
            //  await ExecuteTaskForJobs();
            // await ExecuteTaskForContestants();
            // await Task.Delay(TimeSpan.FromHours(24));
            // }





             while (true)
             {
             await ExecuteTaskForTasksStart();
             await ExecuteUserTasks();
             await Task.Delay(TimeSpan.FromHours(24));
             }


        }
        public async Task ExecuteUserTasks()
        {
            var entityList = _tasksRepository.Set().AsQueryable()
                .Where(x => x.Status == TasksStatus.Approved)
                /*.Select(x => new Tasks
                {
                    Id = x.Id,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    Status = x.Status,
                    Behaviour = x.Behaviour
                })*/.ToAsyncEnumerable();

            if (await entityList.AnyAsync())
            {
                var now = DateTime.Now;

                await foreach (var entity in entityList)
                {
                    if (TimeExtension.IsBetween(now, entity.StartDate, entity.EndDate))
                    {
                        entity.Behaviour = TasksBehaviour.Running;
                        entity.GeneratedLink = $"{_WebUrl}/{Guid.NewGuid()}";
                        _tasksRepository.Update(entity);
                    }
                    else
                    {
                        if (entity.EndDate.Day > DateTime.Now.Day)
                            entity.Behaviour = TasksBehaviour.Ended;
                        if (entity.StartDate.Day < DateTime.Now.Day)
                            entity.Behaviour = TasksBehaviour.Idle;

                        _tasksRepository.Update(entity);
                    }
                }
            

                OperationResult result = await _tasksRepository.SaveChangesAsync();
                if (result.Success)
                {
                    await logger.Create($"Meeting task is hitting start date now", null, LogLevel.Info, null);
                }
           }

        }
        public async Task ExecuteTaskForTasksStart()
        {
            var all = _tasksRepository.Set()
                .Where(t => t.StartDate >= DateTime.Now)
                .Select(x => new Tasks
                {
                    Id = x.Id,
                    Status = x.Status
                })
                .ToAsyncEnumerable();

            if (await all.AnyAsync())
            {
                await foreach (var entity in all)
                {
                    entity.Status = TasksStatus.Waiting;
                    _tasksRepository.Update(entity);
                }

                OperationResult result = await _tasksRepository.SaveChangesAsync();
                if (result.Success)
                {
                    await logger.Create($"Meeting task is hitting start date now", null, LogLevel.Info, null);
                }
            }

        }

        public async Task ExecuteTaskForTasksEnd()
        {
            var all = _tasksRepository.Set()
                .Where(t => t.EndDate >= DateTime.Now)
                .Select(x => new Tasks
                {
                    Id = x.Id,
                    Status = x.Status
                })
                .ToAsyncEnumerable();

            if (await all.AnyAsync())
            {
                await foreach (var entity in all)
                {
                    entity.Status = TasksStatus.Success;
                    _tasksRepository.Update(entity);
                }

                OperationResult result = await _tasksRepository.SaveChangesAsync();
                if (result.Success)
                {
                    await logger.Create($"Meeting task is hitting end date now", null, LogLevel.Info, null);
                }
            }

        }
        public async Task ExecuteTaskForJobs()
        {
            var all = _jobsRepository.Set()
                .Where(t => t.ExpiredOn.Day >= DateTime.Now.Day && t.ExpiredOn.Month >= DateTime.Now.Month && t.ExpiredOn.Year == DateTime.Now.Year && !t.isArchived)
                .Select(x => new Jobs
                {
                    Id = x.Id,
                    isArchived = x.isArchived
                })
                .AsAsyncEnumerable();

            if (!(await all.IsEmptyAsync()))
            {
                await foreach (var entity in all)
                {
                    entity.isArchived = true;
                    _jobsRepository.Update(entity);
                }

                OperationResult result = await _jobsRepository.SaveChangesAsync();
                if(result.Success)
                {
                    await logger.Create($"job posts was archived succesfuly and saved", null, LogLevel.Info, null);
                }
            } 

        }

        public async Task ExecuteTaskForContestants()
        {

            var all = _contestantRepository.Set()
                .Where(t => t.ExpiredOn.Day >= DateTime.Now.Day && t.ExpiredOn.Month >= DateTime.Now.Month && t.ExpiredOn.Year == DateTime.Now.Year && !t.isArchived)
                .Select(x => new Contestant
                {
                    Id = x.Id,
                    isArchived = x.isArchived
                })
                .AsAsyncEnumerable();


            if (!(await all.IsEmptyAsync()))
            {
                await foreach (var entity in all)
                {
                    entity.isArchived = true;
                    _contestantRepository.Update(entity);
                }

                OperationResult result = await _contestantRepository.SaveChangesAsync();
                if (result.Success)
                {
                    await logger.Create($"contestant post was archived succesfuly", null, LogLevel.Info, null);
                }

            }
        }
    }
    public interface IDelayedTask
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}

/*public abstract class BackgroundService : IHostedService, IDisposable
{
    private Task _executingTask;
    private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

    protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        _executingTask = ExecuteAsync(_stoppingCts.Token);

        if (_executingTask.IsCompleted)
        {
            return _executingTask;
        }

        return Task.CompletedTask;
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_executingTask == null)
        {
            return;
        }

        try
        {
            _stoppingCts.Cancel();
        }
        finally
        {
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

    }

    public virtual void Dispose()
    {
        _stoppingCts.Cancel();
 }
}

 public class SystemController : Controller
{
    private readonly RecureHostedService _recureHostedService;

    public SystemController(IHostedService hostedService)
    {
        _recureHostedService = hostedService as RecureHostedService;
    }
    [HttpGet(ApiRoutes.System.Start)]
    public IActionResult Start()
    {
        Console.WriteLine("Start Service");
        _recureHostedService.StartAsync(new CancellationToken());
        return Ok();
    }

    [HttpGet(ApiRoutes.System.Stop)]
    public IActionResult Stop()
    {
        Console.WriteLine("Stop Service");
        Console.WriteLine(_recureHostedService == null);
        _recureHostedService.StopAsync(new CancellationToken());
        return Ok();
    }
}*/