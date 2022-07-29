using HireMe.Entities.Enums;
using System;

namespace HireMe.Web.Areas.ViewModels
{
    public class JobsList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyLogo { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
        public PremiumPackage PremiumPackage { get; set; }
        public ApproveType isApproved { get; set; }
        public bool isArchived { get; set; }
    }
}
