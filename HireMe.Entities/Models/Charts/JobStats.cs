namespace HireMe.Entities.Models.Chart
{
    using HireMe.Entities.Input;
    using System;

    public class JobStats : BaseModelStats
    {
        public int Monday { get; set; }
        public int Тuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public int Sunday { get; set; }

        public void Update(JobStatsInputModel viewModel)
        {
            Id = viewModel.Id;
            DateTime = viewModel.DateTime;
            EntityId = viewModel.EntityId;

            Monday = viewModel.Monday;
            Тuesday = viewModel.Тuesday;
            Wednesday = viewModel.Wednesday;
            Thursday = viewModel.Thursday;
            Friday = viewModel.Friday;
            Saturday = viewModel.Saturday;
            Sunday = viewModel.Sunday;
        }
    }
   
}