namespace HireMe.Entities.Input
{
    using HireMe.Entities.Enums;
    using System.ComponentModel.DataAnnotations;

    public class CreateJobInputModel : BaseInputModel
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Моля въведете заглавие на обява.")]
        [Display(Name = "Заглавие:")]
        [StringLength(50, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 10)]
        public string Name { get; set; }

        [Display(Name = "Местоположение")]
        public string LocationId { get; set; }

        [Display(Name = "Ниво:")]
        public ExprienceLevels ExprienceLevels { get; set; }

        [Display(Name = "Тип:")]
        public JobTypeEnum JobType { get; set; }

        [Required(ErrorMessage = "Моля, въведете кратко описание за вашата обява.")]
        [StringLength(100000, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 100)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Минимално заплащане:")]
        public uint MinSalary { get; set; } = 0;

        [Display(Name = "Максимално заплащане:")]
        public uint MaxSalary { get; set; } = 0;

        [Display(Name = "Вид заплащане:")]
        public SalaryType SalaryType { get; set; }

        public PromotionEnum Promotion { get; set; }

        // [ValidCategoryId]
        [Display(Name = "Избери индустрия")]
        public int CategoryId { get; set; }

        //  [ValidCompanyId]
        [Display(Name = "Избери компания")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Моля въведете точен адрес на фирмата.")]
        [StringLength(60, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 5)]
        [Display(Name = "Адрес")]
        public string Adress { get; set; }

        public string CompanyLogo { get; set; }

        [Display(Name = "Езици")]
        public string LanguageId { get; set; }

        [Display(Name = "Ключови думи")]
        public string TagsId { get; set; }


        public ApproveType isApproved { get; set; }

        public bool isArchived { get; set; }

        [Display(Name = "Вид:")]
        public string WorkType { get; set; }

    }
}
