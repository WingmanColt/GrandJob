using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum AccountType
    {
        [Display(Name = "Не е избрано")]
        None = 0,
        [Display(Name = "Безплатно")]
        Free = 1,
        [Display(Name = "Тест")]
        Test = 2,
        [Display(Name = "PRO Аакаунт")]
        Pro = 3

    }

    
}
 