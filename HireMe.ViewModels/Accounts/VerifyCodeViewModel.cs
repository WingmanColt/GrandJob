namespace HireMe.Web.Controllers
{
    internal class VerifyCodeViewModel
    {
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}