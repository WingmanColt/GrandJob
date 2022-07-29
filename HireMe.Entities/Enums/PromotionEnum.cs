using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum PackageType : int
    {
        [Display(Description = "#ECEDF2")]
        None = 0,
        [Display(Name = "Bronze", Description = "#7A4105", ShortName = "las la-rocket")]
        Bronze = 1,
        [Display(Name = "Silver", Description = "#9D9D9D", ShortName = "lab la-hotjar")]
        Silver = 2,
        [Display(Name = "Gold", Description = "#F1CC06", ShortName = "las la-crown", Prompt = "flash")]
        Gold = 3,
        [Display(Name = "Diamond", Description = "#0DB0FC", ShortName = "las la-gem")]
        Diamond = 4
    }
    public enum PremiumPackage : int
    {
        [Display(Description = "#ECEDF2")]
        None = 0,
        [Display(Name = "Bronze", Description = "#7A4105", ShortName = "lab la-hotjar")]
        Bronze = 1,
        [Display(Name = "Silver", Description = "#9D9D9D", ShortName = "las la-rocket")]
        Silver = 2,
        [Display(Name = "Gold", Description = "#F1CC06", ShortName = "las la-crown", Prompt = "flash")]
        Gold = 3
    }
}