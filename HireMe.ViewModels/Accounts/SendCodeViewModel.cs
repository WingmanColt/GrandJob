namespace HireMe.Web.Controllers
{
    internal class SendCodeViewModel
    {
        public object Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
        public string SelectedProvider { get; internal set; }
    }
}