namespace HireMe.Entities.Models
{
    using Ardalis.GuardClauses;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Company : BaseModel
    {
        public string Title { get; set; }
        public string Email { get; set; }

        public bool isAuthentic_EIK { get; set; }
        public bool Private { get; set; }

        public string About { get; set; }
        public string Logo { get; set; }
        public string GalleryImages { get; set; }
        public string LocationId { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }

        public string Website { get; set; }
        public string Linkdin { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        public double Rating { get; set; }
        public int RatingVotes { get; set; }
        public int VotedUsers { get; set; }
        public int CategoryId { get; set; }

        public string PosterId { get; set; }

        public string Admin1_Id { get; set; }
        public string Admin2_Id { get; set; }
        public string Admin3_Id { get; set; }

        public ApproveType isApproved { get; set; }
        public DateTime Date { get; set; }
        public PromotionEnum Promotion { get; set; }


        [NotMapped]
        public bool isInFavourites { get; set; }

        public void Update(CreateCompanyInputModel viewModel, ApproveType approved, bool authenticEIK, User user)
        {
            Id = viewModel.Id;

            Guard.Against.NullOrEmpty(viewModel.Title, nameof(viewModel.Title));
            Title = viewModel.Title;

            Guard.Against.NullOrEmpty(viewModel.Email, nameof(viewModel.Email));
            Email = viewModel.Email;

            Guard.Against.NullOrEmpty(viewModel.About, nameof(viewModel.About));
            About = viewModel.About;

            Guard.Against.NullOrEmpty(viewModel.Adress, nameof(viewModel.Adress));
            Adress = viewModel.Adress;

            PhoneNumber = viewModel.PhoneNumber;
            LocationId = viewModel.LocationId;
            CategoryId = viewModel.CategoryId;

            Website = viewModel.Website;
            Facebook = viewModel.Facebook;
            Linkdin = viewModel.Linkdin; 
            Twitter = viewModel.Twitter;

            Admin1_Id = viewModel.Admin1_Id;
            Admin2_Id = viewModel.Admin2_Id;
            Admin3_Id = viewModel.Admin3_Id;

            Private = viewModel.Private;
            isAuthentic_EIK = authenticEIK;

            Logo = viewModel.Logo == null ? Logo : viewModel.Logo;
            GalleryImages = viewModel.GalleryImages;

            PosterId = user.Id;
            isApproved = approved;
            Date = DateTime.Now;
        }

    }


}