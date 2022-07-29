namespace HireMe.Entities.Models
{
using Ardalis.GuardClauses;

    public class Category : BaseModel
    {
       /* public Category()
        {
            Jobs = new HashSet<Jobs>();
            Contestants = new HashSet<Contestant>();
        }
*/
        public int JobsCount { get; set; }
        public int CandidatesCount { get; set; }
        public string Title_BG { get; set; }

        public string Icon { get; set; }

       // public virtual ICollection<Jobs> Jobs { get; set; }

      //  public virtual ICollection<Contestant> Contestants { get; set; }

        public void Update(string titleBG, string icon, int jobsc, int candidatesc)
        {
            Guard.Against.NullOrEmpty(titleBG, nameof(titleBG));
            Title_BG = titleBG;

            Icon = icon;

            JobsCount = jobsc;
            CandidatesCount = candidatesc;
        }
    }
}