namespace HireMe.Entities.Input
{
    using HireMe.Entities.Enums;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreatePromotion
    {
        public virtual int Id { get; set; }
        public int productId { get; set; }
        public string UserId { get; set; }
        public PackageType Type { get; set; }
        public PremiumPackage premiumPackage { get; set; }
        public PostType PostType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool BoostedPost { get; set; }
        public bool BoostedPostInHome { get; set; }
        public int RefreshCount { get; set; }
        public bool AutoSuggestion { get; set; }

    }
}
