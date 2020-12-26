using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum Visiblity 
    {
        [Display(Description = "Публично")]
        Public = 0,

        [Display(Description = "Само аз")]
        Private = 1
    }
}