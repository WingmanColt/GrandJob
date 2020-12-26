using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum JobTypeEnum : int
    {
        None = 0,

        [Display(Name = "Пълен работен ден")]
        [Description("(40 часа / седмица)")]
        FullTime = 1,

        [Display(Name = "Договор на час")]
        Hourly = 2,

        [Display(Name = "Фиксирана заплата")]
        FixedPrice = 3
    }

}