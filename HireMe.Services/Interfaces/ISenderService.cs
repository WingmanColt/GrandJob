using SendGrid;
using System.Threading.Tasks;

namespace HireMe.Services.Interfaces
{

    public interface ISenderService
    {
        Task<Response> SendEmailAsync(string email, string subject, string message);
    }
}
