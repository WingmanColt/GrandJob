using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace HireMe.ViewModels.Accounts
{

    public class AccountViewModel 
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Емайл")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}та трябва да е поне {2} и максимум {1} символи.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди паролата")]
        //[Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; }

        [StringLength(20, ErrorMessage = "{0}то трябва да е поне {2} и максимум {1} символи.", MinimumLength = 3)]
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [StringLength(20, ErrorMessage = "{0}та трябва да е поне {2} и максимум {1} символи.", MinimumLength = 3)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Запомни ме")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public bool isEmployer { get; set; }

        public string ErrorMessage { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

    }

}