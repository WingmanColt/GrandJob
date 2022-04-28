using HireMe.Entities.Models;
using System.Collections.Generic;

namespace HireMe.Entities
{ 
    public class BaseInputModel
    {
        public Pager Pager { get; set; }

        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }

        public IAsyncEnumerable<Company> AllCompanies { get; set; }
        public IAsyncEnumerable<Skills> AllTags { get; set; }
        public IAsyncEnumerable<Language> AllLanguages { get; set; }

        public string[] Worktypes { get; set; }

    }
}