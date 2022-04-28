namespace HireMe.Entities.Models.Chart
{
    using HireMe.Entities.Input;
    using System;

    public class Stats : BaseModelStats
    {
        public string AppliedJobsId { get; set; } = "0";
        public string Views { get; set; } = "0";
        public int TotalForYearViews { get; set; }
        /*public int FebruaryViews { get; set; }
        public int MarchViews { get; set; }
        public int AprilViews { get; set; }
        public int MayViews { get; set; }
        public int JuneViews { get; set; }
        public int JulyViews { get; set; }
        public int AugustViews { get; set; }
        public int SeptemberViews { get; set; }
        public int OctomberViews { get; set; }
        public int NovemberViews { get; set; }
        public int DecemberViews { get; set; }*/

        public void Update(StatsInputModel viewModel)
        {
            Id = viewModel.Id;
            DateTime = viewModel.DateTime;
            EntityId = viewModel.EntityId;

            AppliedJobsId = viewModel.AppliedJobsId;
            Views = viewModel.Views;
            TotalForYearViews = viewModel.TotalForYearViews;
        }
    }
   
}