using HireMe.Entities.Models;
using HireMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireMe.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ICompanyService _companyService;

        public DownloadPersonalDataModel(
            UserManager<User> userManager,
            ICompanyService companyService)
        {
            _userManager = userManager;
            _companyService = companyService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Errors/AccessDenied", new { Area = "Identity" });
            }

            if (user.AccountType == 0)
            {
                return RedirectToPage("Pricing");
            }

            /* var personalDataCompanies = new Dictionary<string, string>();
             var personalDataCompanies2 = new Dictionary<string, Dictionary<string, string>>();
             var personalDataProps = typeof(User).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

             if(user.Role == Entities.Enums.Roles.Employer)
             {
                 var company = _companyService.GetAll(user);
                 var last = await company.LastAsync();
                 await foreach (var item in company)
                 {
                     var companyProps = typeof(Company).GetProperties();
                     foreach (var p in companyProps)
                     {
                         personalDataCompanies.Add(p.Name, p.GetValue(item)?.ToString() ?? "null");
                     }

                     personalDataCompanies2.Add("Company: " + item.Title, personalDataCompanies);
                     if (!item.Equals(last))
                     {
                         personalDataCompanies.Clear();
                     }
                     //list.Add()
                 }
             }*/
            /*
            dynamic propValue;
            int number;

            int propCount = personalDataProps.Count();
            var prop = user.GetType().GetProperties();
            //foreach (var item in user)
            // {
            for (int i = 0; i < propCount; i++)
                {
                    propValue = GetPropValue(personalDataProps, prop[i].Name);
                    if (propValue is int ? (Int32.TryParse(propValue.ToString(), out number) && number > 0) : propValue != null)
                    {
                    personalData.Add(prop[i].Name, propValue ?? "null");
                }
                }
           // }*/

            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(User).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            foreach (var p in personalDataProps)
            {
             personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

                    Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData)), "text/json");
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
