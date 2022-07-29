namespace HireMe.Entities.Models.Chart
{
    using HireMe.Entities.Input;
    using System;

    public class CompanyStats : BaseModelStats
    {
        public int January { get; set; }
        public int February { get; set; }
        public int March { get; set; }
        public int April { get; set; }
        public int May { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int August { get; set; }
        public int September { get; set; }
        public int October { get; set; }
        public int November { get; set; }
        public int December { get; set; }

        public void Update(CompanyStatsInputModel viewModel)
        {
            //Id = viewModel.Id;
            DateTime = viewModel.DateTime;
            EntityId = viewModel.EntityId;

            January = viewModel.January;
            February = viewModel.February;
            March = viewModel.March;
            April = viewModel.April;
            May = viewModel.May;
            June = viewModel.June;
            July = viewModel.July;
            August = viewModel.August;
            September = viewModel.September;
            October = viewModel.October;
            November = viewModel.November;
            December = viewModel.December;

        }
    }
    public class CompanyStatsInputModel : BaseModelStats
    {
        public int January { get; set; }
        public int February { get; set; }
        public int March { get; set; }
        public int April { get; set; }
        public int May { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int August { get; set; }
        public int September { get; set; }
        public int October { get; set; }
        public int November { get; set; }
        public int December { get; set; }
    }

}