using HireMe.Entities;
using HireMe.ViewModels.Language;
using HireMe.ViewModels.Skills;
using System.Collections.Generic;

namespace HireMe.ViewModels
{ 
    public class BaseViewModel 
    {
        public Pager Pager { get; set; }
        public Filter Filter { get; set; }


        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }

        public IAsyncEnumerable<Entities.Models.Skills> Skills { get; set; }
        public IAsyncEnumerable<Entities.Models.Language> Languages { get; set; }

        public IAsyncEnumerable<SkillsViewModel> SkillsMapped { get; set; }
        public IAsyncEnumerable<LanguageViewModel> LanguagesMapped { get; set; }
    }

   
}
