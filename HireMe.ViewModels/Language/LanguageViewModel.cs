namespace HireMe.ViewModels.Language
{
    using HireMe.Mapping.Interface;
    using HireMe.Entities.Models;
    public class LanguageViewModel : IMapFrom<Language>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

       


    }

}