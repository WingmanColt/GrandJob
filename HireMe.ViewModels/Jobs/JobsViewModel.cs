namespace HireMe.ViewModels.Jobs
{
    using System.Collections.Generic;
    using System;
    using HireMe.Mapping.Interface;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.ViewModels.Message;
    using Microsoft.AspNetCore.Http;

    public class JobsViewModel : BaseViewModel, IMapFrom<Jobs>/*, IHaveCustomMappings*/
    {
        public int Id { get; set; }

        // Main
        public string Name { get; set; }
        public string LocationId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedOn { get; set; }


        // Details
        public ExprienceLevels ExprienceLevels { get; set; }

        public JobTypeEnum JobType { get; set; }

        public string Adress { get; set; }
        public string Description { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public SalaryType SalaryType { get; set; }

        public PackageType Promotion { get; set; }
        public PremiumPackage PremiumPackage { get; set; }
        public double Rating { get; set; }
        public int RatingVotes { get; set; }
        public int VotedUsers { get; set; }
        public int Views { get; set; }

        public string resumeFilesId { get; set; }
        public string LanguageId { get; set; }
        public string TagsId { get; set; }
        public string WorkType { get; set; }
        public string ReturnUrl { get; set; }
        public virtual IFormFile File { get; set; }

        public int CompanyId { get; set; }
        public string CompanyLogo { get; set; }
        public string PosterID { get; set; }

        public virtual GuestViewModel Guest { get; set; }
        public virtual MessageViewModel Message { get; set; }
        public IAsyncEnumerable<string> GalleryImages { get; set; }

        public IAsyncEnumerable<JobsViewModel> Result { get; set; }
        public IAsyncEnumerable<JobsViewModel> JobsByCompany { get; set; }

        public Company company { get; set; }
        public string GalleryPath { get; set; }
        public bool isInFavourites { get; set; }



      //  [NotMapped]
        //public int RefreshCounter { get; set; }
        /* public void CreateMappings(IProfileExpression configuration)
         {
             configuration.CreateMap<Jobs, JobsViewModel>()
             .ForMember(x => x.Logo, opt => opt.MapFrom(model => Result.GetAsyncEnumerator().));
         }*/
    }



}