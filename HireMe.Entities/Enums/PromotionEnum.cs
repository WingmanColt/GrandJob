using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum PromotionEnum : int
    {
    [Display(Name = "Няма")]
    Default = 0,
    [Display(Name = "Stage 1")]
    Level_1 = 1,
    [Display(Name = "Stage 2")]
    Level_2 = 2,
    [Display(Name = "PRO-Акаунт")]
    AccountUpgrade = 3
    }

}