namespace HireMe.Entities.Input
{
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class StatsInputModel : BaseModelStats
    {
        public string AppliedJobsId { get; set; }
        public string Views { get; set; }
        public int TotalForYearViews { get; set; }
    }
}
