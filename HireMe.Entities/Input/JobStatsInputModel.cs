namespace HireMe.Entities.Input
{
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class JobStatsInputModel : BaseModelStats
    {
        public int Monday { get; set; }
        public int Тuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public int Sunday { get; set; }
    }
}
