using HireMe.ViewModels.Home;
using HireMe.ViewModels.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireMe.ViewModels.PartialsVW
{

    public class ApplyRaportViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string ReturnUrl { get; set; }
        public virtual IFormFile File { get; set; }
        public string resumeFilesId { get; set; }

        public virtual MessageViewModel Message { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }

        [BindProperty]
        public HeaderMenuViewModel HeaderViewModel { get; set; }
    }

}