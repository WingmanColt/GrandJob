namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;

    public interface IFavoritesService
    {
        Task<bool> UpdateFavourite(User user, PostType postType, string postId);
        Task<bool> RemoveAllFavourites(User user);
       // Task<bool> RemoveFromFavourite(User user, PostType postType, string postId);

        IAsyncEnumerable<TViewModel> GetFavouriteBy<TViewModel>(User user, PostType postType);
        Task<int?> GetFavouriteByCount(User user, PostType postType);

        bool isInFavourite(User user, PostType postType, string id);
    }
}
