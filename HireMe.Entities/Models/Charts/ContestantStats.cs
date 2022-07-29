namespace HireMe.Entities.Models.Chart
{
    using HireMe.Entities.Input;
    using System;

    public class ContestantStats : BaseModelStats
    {
        public string ViewsPerDay { get; set; } = "0";


        public void Update(ContestantStatsInputModel viewModel)
        {
            Id = viewModel.Id;
            DateTime = viewModel.DateTime;
            EntityId = viewModel.EntityId;

            ViewsPerDay = viewModel.ViewsPerDay;
        }
    }
    public class ContestantStatsInputModel : BaseModelStats
    {
        public string ViewsPerDay { get; set; }
    }

}