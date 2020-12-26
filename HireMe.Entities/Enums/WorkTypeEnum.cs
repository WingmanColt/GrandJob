using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HireMe.Entities.Enums
{
    public enum WorkType 
    {
        [Display(Name = "Подходяща за начинаещи")]
        Internship = 1,
        [Display(Name = "Пълно работно време")]
        Full = 2,
        [Display(Name = "Непълно работно време")]
        Part = 3,
        [Display(Name = "Временна работа")]
        Temporary = 4,
        [Display(Name = "Подходяща за студенти")]
        Student = 5,
        [Display(Name = "Подходяща за без опитни")]
        NoExperience = 6,
        [Display(Name = "Дистанционно")]
        Remote = 7
    }

    
}
 