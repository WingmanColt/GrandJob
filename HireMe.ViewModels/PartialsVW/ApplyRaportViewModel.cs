namespace HireMe.ViewModels.PartialsVW
{
    public class DeleteViewModel 
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string ReturnUrl { get; set; }
        public string Handler { get; set; }
        public int CurrentPage { get; set; }
        public string Sort { get; set; }

    }
}