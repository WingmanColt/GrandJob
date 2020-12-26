using HireMe.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public class SenderService : ISenderService
    {
        private readonly IConfiguration _configuration;

        public SenderService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task<Response> SendEmailAsync(string email, string subject, string message)
        {
            string apiKey = _configuration["SendGridConf:Key"];
            string from = _configuration["SendGridConf:User"];
            string title = _configuration["SendGridConf:Title"];

            var sendGridClient = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(from, title),
                ReplyTo = new EmailAddress(email),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message     
            };

            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);

            var result = await sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            return result;
        }
    }
}
