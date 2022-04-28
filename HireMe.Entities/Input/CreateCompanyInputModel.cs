namespace HireMe.Entities.Input
{
    using HireMe.Entities.Enums;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateCompanyInputModel : BaseInputModel
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Моля въведете име на фирма.")]
        [Display(Name = "Име на фирма:")]
        [StringLength(50, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 10)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Моля въведете емайл адрес.")]
        [Display(Name = "Емайл:")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Моля, въведете кратко описание за вашата компания.")]
        [StringLength(300, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 10)]
        [Display(Name = "Описание")]
        public string About { get; set; }

        [Display(Name = "Частна компания")]
        public bool Private { get; set; }

        [Display(Name = "Снимка")]
        public string Logo { get; set; }

        [Display(Name = "Местоположение")]
        public string LocationId { get; set; }

        [Required(ErrorMessage = "Моля въведете точен адрес на фирмата.")]
        [StringLength(60, ErrorMessage = "{0} трябва да е между {2} и {1} символи.", MinimumLength = 5)]
        [Display(Name = "Адрес")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Моля въведете телефонен номер за връзка.")]
        [Display(Name = "Телефонен номер:")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Уеб сайт:")]
        public string Website { get; set; }

        [Display(Name = "Linkdin:")]
        public string Linkdin { get; set; }

        [Display(Name = "Facebook:")]
        public string Facebook { get; set; }

        [Display(Name = "Twitter:")]
        public string Twitter { get; set; }

        public string PosterId { get; set; }

        [Display(Name = "ЕИК номер:")]
        public bool isAuthentic_EIK { get; set; }

        [Display(Name = "Персонал:")]
        public string Admin1_Id { get; set; }

        [Display(Name = "Персонал:")]
        public string Admin2_Id { get; set; }

        [Display(Name = "Персонал:")]
        public string Admin3_Id { get; set; }

        public PromotionEnum Promotion { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Лого:")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Галерия:")]
        public string GalleryImages { get; set; }

        [Display(Name = "ЕИК/Булстат:")]
        public string EIK { get; set; }

        public ApproveType isApproved { get; set; }
        public double Rating { get; set; }

        [Display(Name = "Категория:")]
        public int CategoryId { get; set; }

        public List<IFormFile> files { get; set; }

    }
}
