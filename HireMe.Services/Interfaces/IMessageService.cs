namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;

    public interface IMessageService
    { 
        Task<OperationResult> Create(string title, string description, string senderid, string receiverid);

        Task<OperationResult> CreateReport(string title, string description, string senderid, string receiverid);

        Task<OperationResult> Delete(int id, string userId, MessageClient client);

        IAsyncEnumerable<Message> GetMessagesBy(User user, MessageClient client, int MessagesCount);

        IQueryable<Message> GetAllAsNoTracking();

        Task<Message> GetByIdAsync(int id);

        Task<OperationResult> Add_MessageState(int msgId, MessageStates msgState, bool type);

        Task<int> GetMessagesCountBy_Receiver(User user);
    }
}
