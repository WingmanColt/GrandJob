using HireMe.Core.Helpers;
using HireMe.Data;
using HireMe.Entities.Models;
using System.Threading.Tasks;

namespace HireMe.Services.Benchmark
{
    public interface IBenchmarkService
    {
        long RemoveCompanies(User user);
        Task<long> RemoveCompaniesAsync(User user);
        long RemoveContestants(User user);
        Task<long> RemoveContestantsAsync(User user);
        long RemoveJobs(User user);
        Task<long> RemoveJobsAsync(User user);
        long SeedCompany(int count, string posterId);
        Task<long> SeedCompanyAsync(int count, string posterId);
        long SeedContestants(int count, string posterId);
        Task<long> SeedContestantsAsync(int count, string posterId);
        long SeedJobs(int count, string posterId);
        Task<long> SeedJobsAsync(int count, string posterId);
    }
}