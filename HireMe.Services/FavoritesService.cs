namespace HireMe.Services
{
    using HireMe.Core.Extensions;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FavoritesService : IFavoritesService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJobsService _jobsService;
        private readonly IContestantsService _contestantService;
        private readonly ICompanyService _companiesService;

        public FavoritesService(
            UserManager<User> userManager,
            IJobsService jobsService,
            IContestantsService contestantService,
            ICompanyService companiesService)
        {
            _userManager = userManager;
            _jobsService = jobsService;
            _contestantService = contestantService;
            _companiesService = companiesService;
        }

        public async Task<bool> AddToFavourite(User user, PostType postType, string postId)
        {
            if (user is null)
                return false;

                string postIdComplate = ',' + postId;

                switch (postType)
                {
                    case PostType.Company:user.FavouriteCompanies += postIdComplate;     
                        break;
                    case PostType.Contestant:user.FavouriteContestants += postIdComplate;                        
                        break;
                    case PostType.Job:user.FavouriteJobs += postIdComplate;
                        break;
                }

            user.ActivityReaded = false;
            await _userManager.UpdateAsync(user); 
            return true;
        }

        public async Task<bool> RemoveFromFavourite(User user, PostType postType, string postId)
        {
            if (user is null)
                return false;

            string postIdComplate = ',' + postId;

            switch (postType)
            {
                case PostType.Company:
                    {
                        if (user.FavouriteCompanies?.IndexOf(postId) > -1)
                        {
                            user.FavouriteCompanies = user.FavouriteCompanies.Replace(postIdComplate, "");
                        }
                    }
                    break;
                case PostType.Contestant:
                    {
                        if (user.FavouriteContestants?.IndexOf(postId) > -1)
                        {
                            user.FavouriteContestants = user.FavouriteContestants.Replace(postIdComplate, "");
                        }
                    }
                    break;
                case PostType.Job:
                    {
                        if (user.FavouriteJobs?.IndexOf(postId) > -1)
                        {
                            user.FavouriteJobs = user.FavouriteJobs.Replace(postIdComplate, "");
                        }
                    }
                    break;
            }

            await _userManager.UpdateAsync(user);
            return true;
        }

        public IAsyncEnumerable<TViewModel> GetFavouriteBy<TViewModel>(User user, PostType postType)
        {
            if (user is null)
                return null;

            switch (postType)
            {
                case PostType.Company:
                    {
                        string[] items = user?.FavouriteCompanies?.Split(',');
                        if (items is null)
                            return null;

                        var entities = _companiesService.GetAllAsNoTracking()
                        .Where(x => ((IList)items).Contains(x.Id.ToString()))
                        .AsAsyncEnumerable();

                        return (IAsyncEnumerable<TViewModel>)entities;
                    }

                case PostType.Contestant:
                    {
                        string[] items = user?.FavouriteContestants?.Split(',');
                        if (items is null)
                            return null;

                        var entities = _contestantService.GetAllAsNoTracking()
                         .Where(x => ((IList)items).Contains(x.Id.ToString()))
                         .AsAsyncEnumerable();
                       
                        return (IAsyncEnumerable<TViewModel>)entities;
                    }

                case PostType.Job:
                    {
                        string[] items = user?.FavouriteJobs?.Split(',');
                        if (items is null)
                            return null;

                        var entities = _jobsService.GetAllAsNoTracking()
                         .Where(x => ((IList)items).Contains(x.Id.ToString()))
                         .AsAsyncEnumerable();

                        return (IAsyncEnumerable<TViewModel>)entities;
                    }

            }
            return null;
        }

        public async Task<int?> GetFavouriteByCount(User user, PostType postType)
        {
            if (user is null)
                return -1;

            switch (postType)
            {
                case PostType.Company:
                    {
                        var items = user?.FavouriteCompanies?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Skip(1);

                        int? count = items.Count();
                        return count;                            
                    }

                case PostType.Contestant:
                    {
                        var items = user?.FavouriteContestants?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Skip(1);

                        int? count = items.Count();
                        return count;
                    }              
                case PostType.Job:
                    {
                        var items = user?.FavouriteJobs?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Skip(1);

                        int? count = items.Count();
                        return count;
                    }

                case PostType.All:
                    {
                        var itemsCompany = user?.FavouriteCompanies?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Skip(1).Count();
                        var itemsContestant = user?.FavouriteContestants?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Skip(1).Count();
                        var itemsJob = user?.FavouriteJobs?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Skip(1).Count();

                        int? count = itemsCompany + itemsContestant + itemsJob;
                        return count;
                    }

            }
                
            return -1;
        }
        public async Task<bool> isInFavourite(User user, PostType postType, int id)
        {
            if (user is null)
                return false;

            switch (postType)
            {
                case PostType.Company:
                    {
                        var items = user?.FavouriteCompanies?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim())
                            .ToList()
                            .ConvertAll(int.Parse);

                        if (items is null)
                            return false;

                        var entities = items.Any(x => ((IList)items).Contains(id));

                        return entities;
                    }

                case PostType.Contestant:
                    {
                        var items = user?.FavouriteContestants?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim())
                            .ToList()
                            .ConvertAll(int.Parse);

                        if (items is null)
                            return false;

                        var entities = items.Any(x => ((IList)items).Contains(id));

                        return entities;
                    }

                case PostType.Job:
                    {
                        var items = user?.FavouriteJobs?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim())
                            .ToList()
                            .ConvertAll(int.Parse);

                        if (items is null)
                            return false;

                        var entities = items.Any(x => ((IList)items).Contains(id));
                        return entities;
                    }

            }
            return false;
        }
    }
}