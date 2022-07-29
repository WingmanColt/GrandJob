using HireMe.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using System.Collections.Generic;
using System;
using HireMe.Core.Helpers;
using NToastNotify.Helpers;

namespace HireMe.Services
{
    public class SenderService : ISenderService
    {
        public SenderService() { }
        public async Task<OperationResult> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var apiInstance2 = new AccountApi();
                GetAccount result2 = apiInstance2.GetAccount();

                var apiInstance = new TransactionalEmailsApi();
                var templateId = 2;  // long? | Id of the template

                //     var list = new List<SendSmtpEmailTo>();
                var sender = new SendSmtpEmailSender() { Email = "supp.grandjob@gmail.com", Name = "GrandJob" };
                //  list.Add(new SendSmtpEmailTo() { Email = email, Name = email.GetUntilOrEmpty("@") });


                SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(email, email);
                List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
                To.Add(smtpEmailTo);

                var sendEmail = new SendSmtpEmail(sender, To, null, null, null, message, subject, null, null, null, templateId, null, null, null); // SendTestEmail |
                                                                                                                                                   // 
                var apiInstanceContacts = new ContactsApi();


                var contactExists = await apiInstanceContacts.GetContactsAsync();
                var contactList = contactExists.Contacts.ToJson();

                if (!contactList.Contains(email))
                {
                    // Create a contact
                    var createContact = new CreateContact((email), (subject, message));
                    await apiInstanceContacts.CreateContactAsync(createContact);
                }
                else
                {
                    // Update a contact
                    var updateContact = new UpdateContact((subject, message));
                    await apiInstanceContacts.UpdateContactAsync(email, updateContact);
                }

                // Send a template to your test list

                await apiInstance.SendTransacEmailAsync(sendEmail);
                return OperationResult.SuccessResult("");
            }
            catch (Exception e)
            {
                OperationResult.FailureResult($"Email sending to {email} failed! Ex: {e}");
            }

            return OperationResult.FailureResult($"Email sending to {email} failed!");
        }

        /*
        public async Task<CreateSmtpEmail> SendEmailAsync(string email, string subject, string message)
        {
            var apiInstance2 = new AccountApi();

            GetAccount result2 = apiInstance2.GetAccount();
            var apiInstance = new TransactionalEmailsApi();

            var list = new List<SendSmtpEmailTo>();
            list.Add(new SendSmtpEmailTo("supp.gstore@gmail.com"));
            var msg = new SendSmtpEmail()
            {
                To = list,
                TemplateId = 2,
                HtmlContent = "ALABALA",
                TextContent = "amjik",
                Subject = subject
            };


            var result = await apiInstance.SendTransacEmailAsync(msg); //await sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            return result;
        }*/
        /*
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
        }*/
    }
}
