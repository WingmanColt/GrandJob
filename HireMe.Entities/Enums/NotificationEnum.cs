using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum NotifyType : int
    {
        [Display(Name = "Информация")]
        Information = 0,
        [Display(Name = "Внимание")]
        Warning = 1,
        [Display(Name = "Грешка")]
        Danger = 2,
        [Display(Name = "Успешно")]
        Success = 3,
        Image = 4,
        [Display(Name = "Задачи")]
        Tasks = 5,
        [Display(Name = "Активно")]
        Activated = 6
    }

}