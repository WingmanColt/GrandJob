using HireMe.Mapping.Interface;
using HireMe.ViewModels;
using System;

namespace HireMe.Entities.Models
{
    public class ContestantDetailsViewModel : BaseViewModel, IMapFrom<ContestantDetails>
    {
        public int Id { get; set; }
        public string Education { get; set; }
        public string Education_Place { get; set; }
        public DateTime Education_StartDate { get; set; }
        public DateTime Education_EndDate { get; set; }


        public string Work { get; set; }
        public string Work_Place { get; set; }
        public DateTime Work_StartDate { get; set; }
        public DateTime Work_EndDate { get; set; }


        public string Award { get; set; }
        public string Award_Description { get; set; }
        public DateTime Award_StartDate { get; set; }
        public DateTime Award_EndDate { get; set; }
    }
}
