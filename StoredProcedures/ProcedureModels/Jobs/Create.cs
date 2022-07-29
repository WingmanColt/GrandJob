namespace HireMe.StoredProcedures.Models.Jobs
{
    using Ardalis.GuardClauses;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Create
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocationId { get; set; }
        public string WorkType { get; set; }
        public ExprienceLevels ExprienceLevels { get; set; }
        public JobTypeEnum JobType { get; set; }
        public string Adress { get; set; }
        public string Description { get; set; }

        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public SalaryType SalaryType { get; set; }

        public PackageType Promotion { get; set; }
        public PremiumPackage PremiumPackage { get; set; }

        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyLogo { get; set; }

        public string LanguageId { get; set; }
        public string TagsId { get; set; }

        public string PosterID { get; set; }
        public ApproveType isApproved { get; set; }
        public bool isArchived { get; set; }

        [NotMapped]
        public string StatementType { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ExpiredOn { get; set; } = DateTime.Now.AddMonths(1);

        public double Rating { get; set; } = 0.0;
        public int Views { get; set; } = 0;
        public int RatingVotes { get; set; } = 0;
        public int VotedUsers { get; set; } = 0;
        public int ApplyCount { get; set; } = 0;

        public void Update(CreateJobInputModel viewModel, ApproveType approved, User user)
        {
            Id = viewModel.Id;

            Guard.Against.NullOrEmpty(viewModel.Name, nameof(viewModel.Name));
            Name = viewModel.Name;

            Guard.Against.NullOrEmpty(viewModel.Description, nameof(viewModel.Description));
            Description = viewModel.Description;

            Guard.Against.NullOrEmpty(viewModel.Adress, nameof(viewModel.Adress));
            Adress = viewModel.Adress;

            Guard.Against.NullOrEmpty(viewModel.WorkType, nameof(viewModel.WorkType));
            WorkType = viewModel.WorkType;

            ExprienceLevels = viewModel.ExprienceLevels;
            LocationId = viewModel.LocationId;

            if(viewModel.MinSalary > 0)
            MinSalary = viewModel.MinSalary;
            if (viewModel.MaxSalary > 0)
            MaxSalary = viewModel.MaxSalary;

            SalaryType = viewModel.SalaryType;

            Guard.Against.Negative(viewModel.CompanyId, nameof(viewModel.CompanyId));
            CompanyId = viewModel.CompanyId;

            CategoryId = viewModel.CategoryId;
            LanguageId = viewModel.LanguageId;
            TagsId = viewModel.TagsId;

            CompanyLogo = viewModel.CompanyLogo;

            PosterID = user?.Id;
            isApproved = approved;
            isArchived = viewModel.isArchived;

            Promotion = viewModel.Promotion;
            PremiumPackage = viewModel.PremiumPackage;
        }
    }
   
}