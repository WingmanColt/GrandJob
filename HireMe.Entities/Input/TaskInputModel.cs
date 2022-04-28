namespace HireMe.Entities.Input
{
    using HireMe.Entities.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TaskInputModel : BaseModel
    {
        [Required(ErrorMessage = "Моля въведете заглавие на задачата.")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Кратко описание")]
        public string About { get; set; }

        [Required(ErrorMessage = "Моля въведете начална дата.")]
        [Display(Name = "Начална дата")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage = "Моля въведе крайна дата.")]
        [Display(Name = "Крайна дата")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Тип задача")]
        public TaskLevel Level { get; set; }
        public DateTime Date { get; set; }
        public TasksStatus Status { get; set; }
        public TasksBehaviour Behaviour { get; set; }

        public string GeneratedLink { get; set; }

        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
    }
}
