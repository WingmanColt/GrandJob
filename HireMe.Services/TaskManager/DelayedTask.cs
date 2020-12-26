namespace HireMe.Services
{
    using HireMe.Core.Helpers;
    using HireMe.Data.Repository.Interfaces;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class DelayedTask : IDelayedTask
    {
        private readonly ILogService logger;
        private readonly IRepository<Jobs> _jobsRepository;
        private readonly IRepository<Contestant> _contestantRepository;

        public DelayedTask(ILogService logger, 
            IRepository<Jobs> jobsRepository,
            IRepository<Contestant> contestantRepository)
        {
            this.logger = logger;
            _jobsRepository = jobsRepository;
            _contestantRepository = contestantRepository;
        }

        public async Task DoWork(CancellationToken token)
        {        
            
           // while (true)
           // {
              //  await ExecuteTaskForJobs();
               // await ExecuteTaskForContestants();
               // await Task.Delay(TimeSpan.FromHours(24));
            //}

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