namespace HireMe.Entities.Models
{
using System.Collections.Generic;
using Ardalis.GuardClauses;

    public class Category : BaseModel
    {
        public Category()
        {
            Jobs = new HashSet<Jobs>();
            Contestants = new HashSet<Contestant>();
        }

        public string Title { get; set; }

        public string Title_BG { get; set; }

        public string Icon { get; set; }

        public virtual ICollection<Jobs> Jobs { get; set; }

        public virtual ICollection<Contestant> Contestants { get; set; }

        public void Update(string title, string titleBG, string icon)
        {
            Guard.Against.NullOrEmpty(title, nameof(title));
            Title = title;
            Guard.Against.NullOrEmpty(titleBG, nameof(titleBG));
            Title_BG = titleBG;
            Icon = icon;
        }
    }
}