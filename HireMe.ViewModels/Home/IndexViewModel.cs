﻿using HireMe.Entities;
using HireMe.Entities.Models;
using HireMe.ViewModels.Jobs;
using System.Collections.Generic;

namespace HireMe.ViewModels.Home
{

    public class IndexViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool rememberMe { get; set; }

        public int CategoryId { get; set; }
        public string SearchString { get; set; }
        public string LocationId { get; set; }
        public string ReturnUrl { get; set; }
        public string PictureUrl { get; set; }
        public User User { get; set; }

        public IAsyncEnumerable<SelectListModel> AllCategories { get; set; }
        public IAsyncEnumerable<SelectListModel> AllLocations { get; set; }
        public IAsyncEnumerable<Contestant> LastContestants { get; set; }

        public IAsyncEnumerable<Category> TopCategories { get; set; }
        public IAsyncEnumerable<Entities.Models.Company> TopCompanies { get; set; }
        //public IAsyncEnumerable<Entities.Models.Skills> Skills { get; set; }

        public IAsyncEnumerable<JobsViewModel> JobsLast { get; set; }
        public IAsyncEnumerable<JobsViewModel> JobsTop { get; set; }
      //  public IAsyncEnumerable<Entities.Models.Jobs> JobsByCompany { get; set; }

    }
}