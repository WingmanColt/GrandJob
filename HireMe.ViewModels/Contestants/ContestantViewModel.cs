namespace HireMe.ViewModels.Contestants
{
    using HireMe.Entities.Enums;
    using HireMe.Entities.Input;
    using HireMe.Entities.Models;
    using HireMe.Mapping.Interface;
    using HireMe.ViewModels.Message;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;

    public class ContestantViewModel : BaseViewModel, IMapFrom<Contestant>//, IHaveCustomMappings
    {
        public int Id { get; set; }

        // Main
        public string FullName { get; set; }
        public Gender Genders { get; set; }
        public DateTime Age { get; set; }
        public string LocationId { get; set; }

        public ApproveType isApproved { get; set; }
        public bool isArchived { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiredOn { get; set; }

        // Details
        public string About { get; set; }
        public string Speciality { get; set; }
        public string Description { get; set; }
        public int Experience { get; set; }
        public int payRate { get; set; }
        public SalaryType SalaryType { get; set; }
        public int profileVisiblity { get; set; }
        public string WorkType { get; set; }

        public uint profileViews { get; set; }
        public double Rating { get; set; }
        public int RatingVotes { get; set; }
        public int VotedUsers { get; set; }

        public uint Views { get; set; }

        public string ResumeFileId { get; set; }
        public string resumeFullPath { get; set; }
        public string imageFullPath { get; set; }

        // Web presence
        public string Website { get; set; }
        public string Portfolio { get; set; }
        public string Linkdin { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Github { get; set; }
        public string Dribbble { get; set; }
        public string Logo { get; set; }
        public PackageType Promotion { get; set; }
        public PremiumPackage PremiumPackage { get; set; }
        public IFormFile FormFile { get; set; }

        // Links
        public string userSkillsId { get; set; }
        public string LanguagesId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string PosterID { get; set; }

        // Index
        public string SkillsId { get; set; }
        public string Sort { get; set; }
        public string ReturnUrl { get; set; }

        public bool isInFavourites { get; set; }
        public User conUser { get; set; }

        public virtual CreateMessageInputModel MessageInputModel { get; set; }
        public virtual MessageViewModel Message { get; set; }
        public IAsyncEnumerable<ContestantViewModel> Result { get; set; }


        public IAsyncEnumerable<ContestantDetails> ContestantDetails_Educations { get; set; }
        public IAsyncEnumerable<ContestantDetails> ContestantDetails_Works { get; set; }
        public IAsyncEnumerable<ContestantDetails> ContestantDetails_Awards { get; set; }


        // public virtual List<int> MySkills { get; set; } 

        /* public void CreateMappings(IProfileExpression configuration)
         {
             configuration.CreateMap<Contestant, ContestantViewModel>()
            // .ForMember(x => x.userSkillsId, opt => opt.MapFrom(model => model))
             .ForMember(dest => dest.MySkills, opt => opt.MapFrom(src => ToWordsList(src.userSkillsId)));
             //configuration.CreateMap<Contestant, ContestantViewModel>().ReverseMap();
             //.AfterMap((Contestant, ContestantViewModel) => ContestantViewModel.MySkills = ConvertToInt(Contestant.userSkillsId?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList()));
             // .ForMember(x => x.MySkills, opt => opt.MapFrom(x => x.userSkillsId.Split(',', StringSplitOptions.RemoveEmptyEntries)Select(p => p.Trim()).ToList()));
             // ContestantViewModel.MySkills = ContestantViewModel.userSkillsId?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList()
         }

         public static List<int> ToWordsList(string words)
         {
             return string.IsNullOrWhiteSpace(words) ? new List<int>() : words.Split(",").ToList().ConvertAll(int.Parse);
         }
         private List<int> ConvertToInt(List<string> stringList)
         {
             List<int> intList = new List<int>();

             foreach (String s in stringList)
             {
                 intList.Add(int.Parse(s));
             }
             return intList;
         }*/
    }

}