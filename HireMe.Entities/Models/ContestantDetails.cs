using System;

namespace HireMe.Entities.Models
{
    public class ContestantDetails : BaseModel
    {
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

        public string PosterId { get; set; }

        public void Update(ContestantDetailsInputModel viewModel, User user)
        {
            Id = viewModel.Id;

            Education = viewModel.Education;
            Education_Place = viewModel.Education_Place;
            Education_StartDate = viewModel.Education_StartDate;
            Education_EndDate = viewModel.Education_EndDate;

            Work = viewModel.Work;
            Work_Place = viewModel.Work_Place;
            Work_StartDate = viewModel.Work_StartDate;
            Work_EndDate = viewModel.Work_EndDate;

            Award = viewModel.Award;
            Award_Description = viewModel.Award_Description;
            Award_StartDate = viewModel.Award_StartDate;
            Award_EndDate = viewModel.Award_EndDate;

            PosterId = user.Id;
        }
    }
}
