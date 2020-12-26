using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum Gender
    {
        [Display(Name = "Мъж")]
        Male,

        [Display(Name = "Жена")]
        Female
    }
}