namespace HireMe.ViewModels.Company
{
    using HireMe.Mapping.Interface;
    using HireMe.Entities.Models;
    using HireMe.Entities.Enums;
    using System;
    using AutoMapper;

    public class CompanyViewModel : BaseViewModel, IMapFrom<Company>//, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public bool isAuthentic_EIK { get; set; }

        public string About { get; set; }

        public bool Private { get; set; }

        public string Logo { get; set; }

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

        public string PosterId { get; set; }

        public string Admin1_Id { get; set; }

        public string Admin2_Id { get; set; }

        public string Admin3_Id { get; set; }

        public ApproveType isApproved { get; set; }

        public DateTime Date { get; set; }

        public PromotionEnum Promotion { get; set; }

      /*  public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Company, CompanyViewModel>()
           .ForMember(dtoo => dtoo.Date, opt => opt.MapFrom(efo => efo.Date.ToString()));
        }*/
    }
}