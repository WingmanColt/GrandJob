using HireMe.Core.Helpers;
using System.Threading.Tasks;

namespace HireMe.Services.Interfaces
{

    public interface ISenderService
    {
        Task<OperationResult> SendEmailAsync(string email, string subject, string message);
    }
}
