namespace HireMe.ViewModels.Jobs
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;

    public class GuestViewModel 
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public IFormFile File { get; set; }
        public int Answer { get; set; }
        public bool isAgree { get; set; }

    }
    
}