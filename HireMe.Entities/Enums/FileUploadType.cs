using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HireMe.Entities.Enums
{
    public enum FileType : int
    {
        AppliedCV = 0,
        MyFilesCV = 1,
        GuestsCV = 2
    }
    public enum ResumeType
    {
        [Display(Description = "Активен")]
        Active = 0,
        [Display(Description = "Архивиран")]
        Archived = 1,

        Both = 2
    }

}
 