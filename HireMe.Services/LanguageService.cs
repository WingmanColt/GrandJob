namespace HireMe.Services
{
    using HireMe.Core.Helpers;
    using HireMe.Data.Repository.Interfaces;
    using HireMe.Entities.Models;
    using HireMe.Services.Interfaces;
    using HireMe.ViewModels.Contestants;
    using HireMe.ViewModels.Jobs;
    using Microsoft.EntityFrameworkCore;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public class LanguageService : ILanguageService
    {
        private readonly IRepository<Language> LanguageRepository;

        public LanguageService(IRepository<Language> LanguageRepository)
        {
            this.LanguageRepository = LanguageRepository;

        }
        public IAsyncEnumerable<Language> GetAll(string LanguageId)
        {           
            string[] words = LanguageId.Split(',');
            if (words is null)
                return null;

            var entity = GetAllAsNoTracking()
                .Where(x => ((IList)words).Contains(x.Id.ToString()))
                .AsAsyncEnumerable();

            return entity;
        }

        private IQueryable<Language> GetAllAsNoTracking()
        {
            return LanguageRepository.Set().AsNoTracking();
        }

        // Seed lang
        public async Task<OperationResult> SeedLanguages()
        {
            if (await GetAllAsNoTracking().AnyAsync())
                return OperationResult.FailureResult("Languages already exists.");


            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            foreach (CultureInfo culture in cultures)
            {
                var languages = new Language
                {
                    Name = culture.EnglishName,
                    Code = culture.TwoLetterISOLanguageName
                };
                await LanguageRepository.AddRangeAsync(languages);
            }

            var result = await LanguageRepository.SaveChangesAsync();
            return result;
        }
    }
}