using System.Collections.Generic;

namespace HireMe.Entities
{ 
    public class BaseInputModel
    {
        public Pager Pager { get; set; }

        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }
       
    }
}