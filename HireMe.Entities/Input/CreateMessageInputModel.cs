namespace HireMe.Entities.Input
{
    using System.ComponentModel.DataAnnotations;

    public class CreateMessageInputModel : BaseInputModel
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Моля напишете заглавие на съобщението.")]
        [StringLength(50, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 5)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Моля, опишете себе си в описанието.")]
        [StringLength(100000, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 20)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Получател")]
        public string ReceiverId { get; set; }

        public string SearchString { get; set; }


    }
}
