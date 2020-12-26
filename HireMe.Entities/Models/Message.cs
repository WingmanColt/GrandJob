namespace HireMe.Entities.Models
{
    using System;
    public class Message : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime dateTime { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        public bool isRead { get; set; }
        public bool isImportant { get; set; }
        public bool isStared { get; set; }
        public bool isReport { get; set; }

        public bool deletedFromSender  { get; set; }
        public bool deletedFromReceiver { get; set; }


    }


}