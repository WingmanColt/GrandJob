namespace HireMe.Entities.Models
{
    public class Favorites : BaseModel
    {
        public int? JobsId { get; set; }

        public int? CompanyId { get; set; }

        public string ContestantId { get; set; }

        public string UserId { get; set; }

    }


}