using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{

    //DESCRIPTOPN = COMPANY, SHORTNAME = JOBS, PROMPT = CONTESTANTS
    public enum AccountType
    {
        [Display(Name = "Не е избрано")]
        None = 0,
        [Display(Name = "Безплатно", Description = "1", ShortName = "5", Prompt = "1")]
        Free = 1,
        [Display(Name = "Тест", Description = "3", ShortName = "10", Prompt = "2")]
        Test = 2,
        [Display(Name = "PRO Аакаунт", Description = "10", ShortName = "100", Prompt = "3")]
        Pro = 3

    }

    
}
 