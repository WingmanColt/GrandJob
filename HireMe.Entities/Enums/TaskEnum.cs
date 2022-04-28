using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum TaskLevel
    {
        [Display(Name = "Нормална")]
        Normal = 0,
        [Display(Name = "Разговор")]
        Video = 1,

    }

    public enum TasksStatus
    {
        [Display(Name = "В изчакване")]
        Waiting = 0,
        [Display(Name = "Одобрена")]
        Approved = 1,
        [Display(Name = "Отхвърлена")]
        Rejected = 2,
        [Display(Name = "Успешно завършена")]
        Success = 3,
        [Display(Name = "Не завършена")]
        Failed = 4
    }
    public enum TasksBehaviour
    {
        [Display(Name = "В изчакване")]
        Idle = 0,
        [Display(Name = "В процес")]
        Running = 1,
        [Display(Name = "Завършен")]
        Ended = 2
    }
}
 