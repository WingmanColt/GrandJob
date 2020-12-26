using HireMe.Entities;
using HireMe.ViewModels.Language;
using HireMe.ViewModels.Skills;
using System.Collections.Generic;

namespace HireMe.ViewModels
{ 
    public class BaseViewModel 
    {
        public Pager Pager { get; set; }

        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }
        public IAsyncEnumerable<SelectListModel> AllCompanies { get; set; }

        public IAsyncEnumerable<SelectListModel> AllWorktypes { get; set; }

        public IAsyncEnumerable<Entities.Models.Skills> Skills { get; set; }
        public IAsyncEnumerable<Entities.Models.Language> Languages { get; set; }

        public IAsyncEnumerable<SkillsViewModel> SkillsMapped { get; set; }
        public IAsyncEnumerable<LanguageViewModel> LanguagesMapped { get; set; }
    }

   
}
