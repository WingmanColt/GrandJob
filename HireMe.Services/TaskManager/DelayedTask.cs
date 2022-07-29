namespace HireMe.Services
{
    using HireMe.Core.Extensions;
    using HireMe.Core.Helpers;
    using HireMe.Data.Repository.Interfaces;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Entities.Models.Chart;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class DelayedTask : IDelayedTask
    {
        private readonly ILogService logger;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<CompanyStats> _companyStatsRepository;
        private readonly IRepository<JobStats> _jobStatsRepository;
        private readonly IRepository<Jobs> _jobsRepository;
        private readonly IRepository<Contestant> _contestantRepository;
        private readonly IRepository<Tasks> _tasksRepository;
        private readonly string _WebUrl;

        public DelayedTask(
            IConfiguration config,
            ILogService logger,
            IRepository<CompanyStats> companyStatsRepository,
            IRepository<JobStats> jobStatsRepository,
            IRepository<Company> companyRepository,
            IRepository<Jobs> jobsRepository,
            IRepository<Contestant> contestantRepository,
            IRepository<Tasks> tasksRepository)
        {
            this.logger = logger;
            _companyStatsRepository = companyStatsRepository;
            _jobStatsRepository = jobStatsRepository;
            _companyRepository = companyRepository;
            _jobsRepository = jobsRepository;
            _contestantRepository = contestantRepository;
            _tasksRepository = tasksRepository;

            _WebUrl = config.GetValue<string>("MySettings:SiteWebCamUrl");
        }

        public async Task DoWork(CancellationToken token)
        {
         //    while (true)
         //    {
           /// await ExecuteTaskForTasksStart();
            // await ExecuteUserTasks();
            // await Task.Delay(TimeSpan.FromHours(24));
            // }
        }


        public async Task DoWorkMonthly(CancellationToken token)
        {
            while (true)
            {
                //await ExecuteCompanyStatistics(token);
                await ExecuteJobStatistics(token);
                await Task.Delay(TimeSpan.FromHours(24));
               // await Task.Delay(TimeSpan.FromDays(30));
            }


        }

        // Companies
        public async Task ExecuteCompanyStatistics(CancellationToken stoppingToken)
        {
            var entityList = _companyRepository.Set().AsQueryable()
                .Where(x => x.isApproved == ApproveType.Success);
               // .AsAsyncEnumerable();


            if (await entityList.AnyAsync())
            {
                foreach (var entity in entityList)
                {          
                   await UpdateCompanyData(entity, entity.Views);
                }  
            }
        }


        public async Task UpdateCompanyData(Company entity, int Views)
        {
            try
            {
                CompanyStats IsExists = await GetByIdAsync(entity.Id);

                var companyEntity = new CompanyStatsInputModel();

                switch (DateTime.Now.Month)
                {
                    case 1: companyEntity.January = Views; break;
                    case 2: companyEntity.February = Views; break;
                    case 3: companyEntity.March = Views; break;
                    case 4: companyEntity.April = Views; break;
                    case 5: companyEntity.May = Views; break;
                    case 6: companyEntity.June = Views; break;
                    case 7: companyEntity.July = Views; break;
                    case 8: companyEntity.August = Views; break;
                    case 9: companyEntity.September = Views; break;
                    case 10: companyEntity.October = Views; break;
                    case 11: companyEntity.November = Views; break;
                    case 12: companyEntity.December = Views; break;
                }

                companyEntity.EntityId = entity.Id;
               // companyEntity.PosterId = entity.PosterId;
                companyEntity.January = 20;
                companyEntity.December = 50;

                if (IsExists is null)
                {
                    CompanyStats newEntity = new CompanyStats();

                    newEntity.PosterId = entity.PosterId;
                    newEntity.Update(companyEntity);

                    await _companyStatsRepository.AddAsync(newEntity);
                }
                else
                {
                    if (companyEntity is not null)
                    {
                        IsExists.Update(companyEntity);
                        _companyStatsRepository.Update(IsExists);
                    }
                }

                 await _companyStatsRepository.SaveChangesAsync();
                //return result;
            }
            catch (Exception)
            {

                throw;
            }
    }


        // Jobs
        public async Task ExecuteJobStatistics(CancellationToken stoppingToken)
        {
            var entityList = _jobsRepository.Set().AsQueryable()
                .Where(x => x.isApproved == ApproveType.Success);

            if (await entityList.AnyAsync())
            {
                foreach (var entity in entityList)
                {
                    await UpdateJobData(entity, entity.Views);
                }
            }
        }

        public async Task UpdateJobData(Jobs entity, int Views)
        {
            try
            {
                var IsExists = await GetByIdJobAsync(entity.Id);

                var jobEntity = new JobStatsInputModel();
                jobEntity.EntityId = entity.Id;

               // if(Views > 0)
               // jobEntity.ViewsPerDay =  IsExists.ViewsPerDay.EndsWith(',') ? Views.ToString() : ',' + Views.ToString();

                jobEntity.ViewsPerDay = "10,20,50";
                jobEntity.PosterId = entity.PosterID;
                if (IsExists is null)
                {
                    JobStats newEntity = new JobStats();

                    newEntity.PosterId = entity.PosterID;
                    newEntity.Update(jobEntity);

                    await _jobStatsRepository.AddAsync(newEntity);
                }
                else
                {
                    if (jobEntity is not null)
                    {
                        IsExists.Update(jobEntity);
                        _jobStatsRepository.Update(IsExists);
                    }
                }

                await _jobStatsRepository.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<CompanyStats> GetByIdAsync(int id)
        {
            var ent = await _companyStatsRepository.Set().Where(p => p.EntityId == id).FirstOrDefaultAsync();
            return ent;
        }
        private async Task<JobStats> GetByIdJobAsync(int id)
        {
            var ent = await _jobStatsRepository.Set().Where(p => p.EntityId == id).FirstOrDefaultAsync();
            return ent;
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
        Task DoWorkMonthly(CancellationToken stoppingToken);
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