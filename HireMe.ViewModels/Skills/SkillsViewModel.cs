namespace HireMe.ViewModels.Skills
{
    using HireMe.Mapping.Interface;
    using HireMe.Entities.Models;

    public class SkillsViewModel : IMapFrom<Skills>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}