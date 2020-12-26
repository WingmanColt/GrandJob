using HireMe.Entities;
using HireMe.Entities.Models;
using System.Collections.Generic;

namespace HireMe.ViewModels.Home
{

    public class IndexViewModel
    {
        public int CategoryId { get; set; }
        public string SearchString { get; set; }
        public string ReturnUrl { get; set; }
        public string PictureUrl { get; set; }
        public User User { get; set; }
        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<Contestant> LastContestants { get; set; }

        public IAsyncEnumerable<Category> TopCategories { get; set; }
        public IAsyncEnumerable<Entities.Models.Company> TopCompanies { get; set; }
    }
}