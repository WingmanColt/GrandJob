using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using HireMe.Core.Helpers;
using HireMe.Entities.Models;
using HireMe.Entities.Enums;
using Microsoft.EntityFrameworkCore;


namespace HireMe.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> messageRepository;
        public MessageService(IRepository<Message> messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        public async Task<OperationResult> Create(string title, string description, string senderid, string receiverid)
        {
            if (await IsMessageExists(senderid, title, description))
            {
                return OperationResult.FailureResult("Вече сте изпратили това съобщение!");
            }

            if (title == null || description == null || senderid == null || receiverid == null)
            {
                return OperationResult.FailureResult("Моля въведете всички полета.");
            }

            if (receiverid == senderid)
            {
                return OperationResult.FailureResult("Не можете да изпратите съобщение на себе си.");
            }

            var message = new Message
            {
                Title = title,
                Description = description,
                dateTime = DateTime.Now,
                SenderId = senderid,
                ReceiverId = receiverid,
                isRead = false,
                isImportant = false,
                isStared = false
               
            };

            await messageRepository.AddAsync(message);

            var result = await messageRepository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> CreateReport(string title, string description, string senderid, string receiverid)
        {
            var message = new Message
            {
                Title = title,
                Description = description,
                dateTime = DateTime.Now,
                SenderId = senderid,
                ReceiverId = receiverid,
                isReport = true
            };

            await this.messageRepository.AddAsync(message);

            var result = await messageRepository.SaveChangesAsync();
            return result;
        }
        public async Task<OperationResult> Delete(int iD, string userid, MessageClient client)
        {
            Message msg = await this.messageRepository.GetByIdAsync(iD);
            
            var success = await PostdeleteAsync(msg, client);
            return success;
        }

        private async Task<OperationResult> PostdeleteAsync(Message msg, MessageClient client)
        {
            if (!msg.isReport || msg.deletedFromReceiver && msg.deletedFromSender)
            {
                messageRepository.Delete(msg);

                var deleted = await messageRepository.SaveChangesAsync();
                return deleted;
            }
            switch (client)
            {
                case MessageClient.Both:
                    {                      
                            messageRepository.Delete(msg);
                    }
                    break;
                case MessageClient.Sender:
                    {
                        if (msg.deletedFromReceiver)
                            messageRepository.Delete(msg);
                        else
                        {
                            msg.deletedFromSender = true;
                            messageRepository.Update(msg);
                        }
                    }
                    break;
                case MessageClient.Receiver:
                    {
                        if (msg.deletedFromSender)
                            messageRepository.Delete(msg);
                        else
                        {
                            msg.deletedFromReceiver = true;
                            messageRepository.Update(msg);
                        }
                    }
                    break;
            }


            var result = await messageRepository.SaveChangesAsync();
            return result;
        }
        public IAsyncEnumerable<Message> GetMessagesBy(User user, MessageClient client, int MessagesCount)
        {
            if (user is null)
                return null;

            switch (client)
            {
                case MessageClient.Sender:
                    {
                        var entity = GetAllAsNoTracking()
                        .Where(j => j.SenderId == user.Id)
                        .OrderByDescending(x => x.dateTime)
                        .Take(MessagesCount)
                        .ToAsyncEnumerable();

                        return entity;
                    }
                

                case MessageClient.Receiver:
                    {
                         var entity = GetAllAsNoTracking()
                         .Where(j => j.ReceiverId == user.Id)
                         .OrderByDescending(x => x.dateTime)
                         .Take(MessagesCount)
                         .ToAsyncEnumerable();

                        return entity;
                    }
                
            }

            return null;
        }
        
        public IQueryable<Message> GetAllAsNoTracking()
        {
            return messageRepository.Set().AsNoTracking();
        }

        public async Task<Message> GetByIdAsync(int id)
        {
            var ent = await messageRepository.Set().Where(p => p.Id == id).FirstOrDefaultAsync();

            return ent;
        }


        public async Task<OperationResult> Add_MessageState(int msgId, MessageStates msgState, bool type)
        {
            var msg = await this.messageRepository.Set().FirstOrDefaultAsync(j => j.Id == msgId);

                switch (msgState)
                {
                    case MessageStates.Read:
                        msg.isRead = type;
                        break;
                    case MessageStates.Stared:
                        msg.isStared = type;
                        break;
                    case MessageStates.Important:
                        msg.isImportant = type;
                        break;
                    case MessageStates.Report:
                        msg.isReport = type;
                        break;
            }
            messageRepository.Update(msg);

            var result = await messageRepository.SaveChangesAsync();
            return result;
        }

        public async Task<int> GetMessagesCountBy_Receiver(User user)
        {
            if (user is null)
                return -1;

            int count = await GetAllAsNoTracking()
                .Where(j => j.ReceiverId == user.Id)
                .CountAsync()
                .ConfigureAwait(false);

            return count;
        }
        private async Task<bool> IsMessageExists(string userId, string title, string desc)
        {
            var result = await messageRepository.Set().AsNoTracking().AnyAsync(x => (x.SenderId == userId) && (x.Title == title) && (x.Description == desc));
            return result;
        }
        /* public async ValueTask<int> GetMessagesCount(string Id, MessageStates State, bool isTrue)
         {
             var ent = await GetAllAsNoTrackingMapped().AsAs().ConfigureAwait(false);

             switch (State)
             {
                 case MessageStates.Read:
                     ent.Where(j => j.isRead == isTrue);
                     break;
                 case MessageStates.Stared:
                     ent.Where(j => j.isStared == isTrue);
                     break;
                 case MessageStates.Important:
                     ent.Where(j => j.isImportant == isTrue);
                     break;
                 case MessageStates.Report:
                     ent.Where(j => j.isReport == isTrue);
                     break;
             }

             var result = ent.CountAsync().ConfigureAwait(false);

             return result;
         }*/
    }
}