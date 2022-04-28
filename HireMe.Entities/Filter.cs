using HireMe.Entities.View;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireMe.Entities
{

    public class Filter : BaseModel
    {
        public Filter()
        {

        }

        [FromQuery]
        public int currentPage { get; set; }

        // [FromQuery]
        public string SearchString { get; set; }
       // [FromQuery]
        public int CategoryId { get; set; }
       // [FromQuery]
        public string LocationId { get; set; }
       // [FromQuery]
        public int CompanyId { get; set; }
       // [FromQuery]
        public string LanguageId { get; set; }
       // [FromQuery]
        public string TagsId { get; set; }
        //[FromQuery]
        public string Sort { get; set; }


        //[FromQuery]
        public int MinSalary { get; set; }
        // [FromQuery]
        public int MaxSalary { get; set; } = 100000;


        public List<CheckBoxListItem> Equipments { get; set; }

        public List<CheckBoxListItem> Exprience { get; set; }

        public List<CheckBoxListItem> SortBy { get; set; } = new List<CheckBoxListItem>()
        {
            new CheckBoxListItem { Value = "Рейтинг", Key = 1 },
            new CheckBoxListItem { Value = "Последни", Key = 2 },
            new CheckBoxListItem { Value = "Най-нови", Key = 3 },
            new CheckBoxListItem { Value = "Заплата", Key = 4 },
        };

        public List<CheckBoxListItem> Salaries { get; set; } = new List<CheckBoxListItem>()
        {
            new CheckBoxListItem { intValue = 500, Value = "500-1000", Key = 1 },
            new CheckBoxListItem { intValue = 1000, Value = "1000-3000", Key = 2 },
            new CheckBoxListItem { intValue = 3000, Value = "3000-5000", Key = 3 },
            new CheckBoxListItem { intValue = 5000, Value = "5000-9000", Key = 4 },
            new CheckBoxListItem { intValue = 9000, Value = "9000-20000", Key = 5 }
        };
    }

}