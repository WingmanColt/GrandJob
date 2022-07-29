using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireMe.StoredProcedures.Enums
{
    public enum JobCrudActionEnum : int
    {
        None = 0,

        [Display(Name = "Create")]
        Create = 1,

        [Display(Name = "Update")]
        Update = 2,

        [Display(Name = "Delete")]
        Delete = 3,

        [Display(Name = "UpdatePromotion")]
        UpdatePromotion = 4,

        [Display(Name = "RefreshDate")]
        RefreshDate = 5,

        [Display(Name = "UpdateUser")]
        UpdateUser = 6
    }

    public enum JobGetActionEnum : int
    {
        None = 0,

        [Display(Name = "GetAllFiltering")]
        GetAllFiltering = 1,

        [Display(Name = "GetAllBy")]
        GetAllBy = 2,

        [Display(Name = "GetAllForDashboard")]
        GetAllForDashboard = 3
    }
}
