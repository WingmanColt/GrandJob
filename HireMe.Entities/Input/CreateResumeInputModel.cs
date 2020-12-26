namespace HireMe.Entities.Input
{
    using Microsoft.AspNetCore.Http;

    public class CreateResumeInputModel : BaseModel
    {
        public string Title { get; set; }

        public string FileId { get; set; }

        public IFormFile FormFile { get; set; }

    }
}
