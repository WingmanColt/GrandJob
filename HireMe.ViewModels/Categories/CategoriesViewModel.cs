namespace HireMe.ViewModels.Categories
{
    using HireMe.Entities.Models;
    using HireMe.Mapping.Interface;
    using System.Collections.Generic;


    public class CategoriesViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Title_BG { get; set; }

        public string Icon { get; set; }

        public IAsyncEnumerable<Category> List { get; set; }    
    }
}