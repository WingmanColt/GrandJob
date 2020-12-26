namespace HireMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HireMe.Core.Helpers;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;

    public interface IFavoritesService
    {
        Task<bool> AddToFavourite(User user, PostType postType, string postId);
        Task<bool> RemoveFromFavourite(User user, PostType postType, string postId);

        IAsyncEnumerable<TViewModel> GetFavouriteBy<TViewModel>(User user, PostType postType);
        Task<int?> GetFavouriteByCount(User user, PostType postType);

        Task<bool> isInFavourite(User user, PostType postType, int id);
    }
}
