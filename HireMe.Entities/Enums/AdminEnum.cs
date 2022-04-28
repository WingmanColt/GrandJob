using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.Enums
{
    public enum LogLevel
    {
     Info = 1,
     Warning = 2,
     Danger = 3
    }

    public enum PostType : int
    {
        Company = 0,
        Contestant = 1,
        Job = 2,
        All = 3
    }

    public enum ApproveType : int
    {
        Waiting = 0,
        Rejected = 1,
        Success = 2,
        None = 3
    }
    public enum Roles : int
    {
        [Display(Name = "Потребител", Description = "#177dff", ShortName = "User")]
        User = 0,
        [Display(Name = "Кандидат", Description = "#ea8323", ShortName = "Contestant")]
        Contestant = 1,
        [Display(Name = "Персонал", Description = "#ff006e", ShortName = "Recruiter")]
        Recruiter = 2,
        [Display(Name = "Работодател", Description = "#9e02e0", ShortName = "Employer")]
        Employer = 3,
        [Display(Name = "Модератор", Description = "#16b92b", ShortName = "Moderator")]
        Moderator = 4,
        [Display(Name = "Администратор", Description = "#c72d2d", ShortName = "Admin")]
        Admin = 5
            

    }

}