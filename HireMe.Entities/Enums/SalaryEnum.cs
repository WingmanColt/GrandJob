using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum SalaryType
    {
        [Display(Name = "месец")]
        month,
        [Display(Name = "година")]
        year,
        [Display(Name = "ден")]
        day,
        [Display(Name="час")]
        hour
    }
}