using System.ComponentModel.DataAnnotations;

namespace HireMe.ViewModels.Accounts
{
    public class ExternalViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Аз съм работодател")]
        public bool IsEmployer { get; set; }

        [EmailAddress]
        public string EmailFromSocial { get; set; }

        public string PictureName { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }
    }
}