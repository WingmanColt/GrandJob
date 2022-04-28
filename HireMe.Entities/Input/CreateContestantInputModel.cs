namespace HireMe.Entities.Input
{
    using HireMe.Entities.Enums;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateContestantInputModel : BaseInputModel
    {
        public virtual int Id { get; set; }

        // Main
        [Display(Name = "Име и фамилия")]
        public string FullName { get; set; }

        [Display(Name = "Пол")]
        public Gender Genders { get; set; }

        [Display(Name = "Населено място")]
        public string LocationId { get; set; }

        [Required(ErrorMessage = "Моля въведи рождената си дата.")]
        [Display(Name = "Рождена дата")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? Age { get; set; }

        // Details
        [Required(ErrorMessage = "Моля, опишете кратко с какво се занимавате.")]
        [StringLength(100, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 10)]
        [Display(Name = "Кратко описание:")]
        public string About { get; set; }

        // Details
        [Required(ErrorMessage = "Моля, напишете специалността си (Инжинер, Механик...)")]
        [StringLength(20, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 3)]
        [Display(Name = "Специалност:")]
        public string Speciality { get; set; }

        [Required(ErrorMessage = "Моля, опишете себе си в описанието.")]
        [StringLength(100000, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 20)]
        [Display(Name = "Описание:")]
        public string Description { get; set; }

       // [Range(0, 100)]
        [Display(Name = "Опит")]
        public int Experience { get; set; }

       // [Range(0, 9999)]
        [Display(Name = "Заплащане (лв)")]
        public int payRate { get; set; }

        [Display(Name = "Вид заплащане")]
        public SalaryType SalaryType { get; set; }

        [Display(Name = "Видимост на профила")]
        public int profileVisiblity { get; set; }

        [Display(Name = "Тип работа")]
        public string WorkType { get; set; }


        // Web presence

        [Display(Name = "Страница")]
        [StringLength(20, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 3)]
        public string Website { get; set; }

        [Display(Name = "Блог")]
        [StringLength(20, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 3)]

        public string Portfolio { get; set; }

        [Display(Name = "Linkedin")]
        [StringLength(20, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 3)]

        public string Linkdin { get; set; }

        [Display(Name = "Facebook")]
        [StringLength(20, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 3)]

        public string Facebook { get; set; }

        [Display(Name = "Twitter")]
        [StringLength(20, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 3)]

        public string Twitter { get; set; }


        [Display(Name = "Github")]
        [StringLength(20, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 3)]

        public string Github { get; set; }


        [Display(Name = "Dribbble")]
        [StringLength(20, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 3)]

        public string Dribbble { get; set; }

        public PromotionEnum Promotion { get; set; }

        // Resume
        [Display(Name = "Резюме")]
        public string ResumeFileId { get; set; }

        // Links
        // [ValidSkillsId]
        [Display(Name = "Умения")]
        public string userSkillsId { get; set; }

        [Display(Name = "Езици")]
        public string LanguagesId { get; set; }

        //  [ValidCategoryId]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        [Display(Name = "Резюме")]
        public IFormFile FormFile { get; set; }

        public string PosterID { get; set; }
        public ApproveType isApproved { get; set; }
        public bool isArchived { get; set; }

        // Custom
        public double Rating { get; set; }
        public uint Views { get; set; }
        public int VotedUsers { get; set; }

        public string Logo { get; set; }

    }

}
