using HireMe.Core.Extensions;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireMe.Controllers.Api
{
    public class ChartApiController : BaseController
    {
        private readonly IJobStatisticsService _jobstatisticsService;
        private readonly IStatisticsService _statisticsService;

        public ChartApiController(
            IJobStatisticsService jobstatisticsService,
            IStatisticsService statisticsService)
        {
            _jobstatisticsService = jobstatisticsService;
            _statisticsService = statisticsService;
        }

        [Authorize]
        [Produces("application/json")]
        public async Task<JsonResult> GetChartDataById(int selectItemId)
        {
            var companyItem = await _statisticsService.GetByCompanyIdAsync(selectItemId);

            var dict = new Dictionary<string, int>();

            string[] monthsLabel = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                 for (int i = 0; i < monthsLabel.Length; i++)
                 {
                   var prop = GetPropValue(companyItem, monthsLabel[i]);
                   dict.Add(monthsLabel[i], Convert.ToInt32(prop));                             
                 }
                        
            return Json(dict);
        }

        [Authorize]
        [Produces("application/json")]
        public async Task<JsonResult> GetChartDataByIdJob(int selectItemId)
        {
            var Item = await _jobstatisticsService.GetByIdAsync(selectItemId);

            var dict = new Dictionary<int, string>();

            // string[] monthsLabel = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int days = DateTimeDayOfMonthExtensions.DaysInMonth(DateTime.Now);
            string[] views = Item.ViewsPerDay?.Split(',');


            int counter = 0;
            for (int j = 0; j < views.Length; j++)
            {
                counter++;

                dict.Add(j, views[j]);

            }
            for (int i = counter; i < days; i++)
            {
                dict.Add(i, "0");
            }

            return Json(dict);
        }
        public static object GetPropValue(object src, string propName)
        {
            if (src is not null)
            return src.GetType().GetProperty(propName).GetValue(src, null);

            return null;
        }

    }
}
