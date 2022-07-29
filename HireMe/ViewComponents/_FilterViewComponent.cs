using HireMe.Entities.View;
using HireMe.Entities;
using HireMe.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.ViewComponents
{
    [ViewComponent(Name = "_FilterJobs")]
    public class _FilterJobsViewComponent : ViewComponent
    {
        public _FilterJobsViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new Filter();

            model.Equipments = Enum.GetValues(typeof(WorkType))
            .Cast<WorkType>()
            .Select(t => new CheckBoxListItem
            {
                Key = ((int)t),
                Value = t.GetDisplayName()
            }).ToList();


            model.Exprience = Enum.GetValues(typeof(ExprienceLevels))
            .Cast<ExprienceLevels>()
            .Select(t => new CheckBoxListItem
            {
                Key = ((int)t),
                Value = t.GetDisplayName()
            }).ToList();


            return View(model);
        }

    }

    [ViewComponent(Name = "_FilterCandidates")]
    public class _FilterCandidatesViewComponent : ViewComponent
    {
        public _FilterCandidatesViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new Filter();

            model.Exprience = Enum.GetValues(typeof(ExprienceLevels))
            .Cast<ExprienceLevels>()
            .Select(t => new CheckBoxListItem
            {
                Key = ((int)t),
                Value = t.GetDisplayName()
            }).ToList();


            return View(model);
        }

    }

    [ViewComponent(Name = "_FilterCompanies")]
    public class _FilterCompaniesViewComponent : ViewComponent
    {
        public _FilterCompaniesViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new Filter();

            return View(model);
        }

    }
}