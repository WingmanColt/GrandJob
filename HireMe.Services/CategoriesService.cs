namespace HireMe.Services
{
    using HireMe.Core.Helpers;
    using HireMe.Data.Repository.Interfaces;
    using HireMe.Entities;
    using HireMe.Entities.Enums;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> categoriesRepository;

        public CategoriesService(IRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }
        public async Task<OperationResult> Update(int categoryId, bool isJobOrCandidate, CategoriesEnum categories)
        {
            var entity = await GetByIdAsync(categoryId);

            int jCounter = entity.JobsCount, cCounter = entity.CandidatesCount;


            if (isJobOrCandidate)
            {
                switch(categories)
                {
                    case CategoriesEnum.Increment:
                        jCounter = jCounter + 1;
                        break;
                    case CategoriesEnum.Decrement:
                        if(jCounter > 0)
                        jCounter = jCounter - 1;
                        break;
                }
            }
            else
            {
                switch (categories)
                {
                    case CategoriesEnum.Increment:
                        cCounter = cCounter + 1;
                        break;
                    case CategoriesEnum.Decrement:
                        if (cCounter > 0)
                            cCounter = cCounter - 1;
                        break;
                }
            }

            entity.Update(entity.Title_BG, entity.Icon, jCounter, cCounter);

            categoriesRepository.Update(entity);

            var result = await categoriesRepository.SaveChangesAsync();
            return result;
        }
        public IAsyncEnumerable<SelectListModel> GetAllSelectList()
        {
            var result = GetAllAsNoTracking()
                    .Select(x => new SelectListModel
                    {
                        Value = x.Id.ToString(),
                        Text = x.Title_BG
                    })
                    .ToAsyncEnumerable();

            return result;
        }

        public IAsyncEnumerable<Category> GetTop(int entitiesToShow)
        {
            return GetAllAsNoTracking()
                   .Take(entitiesToShow)
                   .AsAsyncEnumerable();
        }

        public async Task<string> GetNameById(int categoryId)
        {
            var result = await GetByIdAsync(categoryId);
            return result?.Title_BG?.Length > 0 ? result?.Title_BG : string.Empty;
        }
        public IQueryable<Category> GetAllAsNoTracking()
        {
            return categoriesRepository.Set().AsNoTracking();
        }

        // Seed Categories
        public async Task<OperationResult> SeedCategories()
        {
            if (await GetAllAsNoTracking().AnyAsync())
               await DeleteCategories();
               // return OperationResult.FailureResult("Categories already exists.");

            var lines = await File.ReadAllLinesAsync(@"wwwroot/Categories.txt");

            for (int i = 1; i <= (lines?.Length - 1); i++)
            {
               var vals1 = lines[i]?.Split('#');

                var category = new Category
                {
                        //Title = vals1[0].ToString(),
                        Title_BG = vals1[1].ToString(),
                        Icon = vals1[2].ToString()
                };
                await categoriesRepository.AddAsync(category);
            }

            var result = await categoriesRepository.SaveChangesAsync();
            return result;
        }
        private async Task<OperationResult> DeleteCategories()
        {
            var items = await GetAllAsNoTracking().ToListAsync();

           foreach (var item in items)
            {
                categoriesRepository.Delete(item);
            }

            var result = await categoriesRepository.SaveChangesAsync();
            return result;
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            var ent = await categoriesRepository.Set().FirstOrDefaultAsync(j => j.Id == id);
            return ent;
        }
    }
}