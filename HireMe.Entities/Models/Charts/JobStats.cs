namespace HireMe.Entities.Models.Chart
{
    using HireMe.Entities.Input;
    using System;

    public class JobStats : BaseModelStats
    {
        public string ViewsPerDay { get; set; } = "0";


        public void Update(JobStatsInputModel viewModel)
        {
            DateTime = viewModel.DateTime;
            EntityId = viewModel.EntityId;

            ViewsPerDay = viewModel.ViewsPerDay;
        }
    }
    public class JobStatsInputModel : BaseModelStats
    {
        public string ViewsPerDay { get; set; }
    }

}