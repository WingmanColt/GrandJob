namespace HireMe.Services
{
    using HireMe.Core.Helpers;
    using HireMe.Data.Repository.Interfaces;
    using HireMe.Entities.Models;
    using HireMe.Mapping.Utility;
    using HireMe.Services.Interfaces;
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
        public IAsyncEnumerable<T> GetAll<T>(string LanguageId, bool isMapped)
        {
            string[] words = LanguageId?.Split(',');
            if (words is null)
                return null;

            if (isMapped)
            {
                var entity = GetAllAsNoTracking()
                .Where(x => ((IList)words).Contains(x.Id.ToString()))
                .To<T>()
                .AsAsyncEnumerable();

                return entity;
            }
            else
            {
                var entity = GetAllAsNoTracking()
                .Where(x => ((IList)words).Contains(x.Id.ToString()))
                .AsAsyncEnumerable();

                return (IAsyncEnumerable<T>)entity;
            }

        }
        public IAsyncEnumerable<T> GetAllById<T>(string LanguageId, bool isMapped)
        {
            string[] words = LanguageId?.Split(',');
            if (words is null)
                return null;

            if (isMapped)
            {
                var entity = GetAllAsNoTracking()
                .Where(x => ((IList)words).Contains(x.Name))
                .To<T>()
                .AsAsyncEnumerable();

                return entity;
            }
            else
            {
                var entity = GetAllAsNoTracking()
                .Where(x => ((IList)words).Contains(x.Name))
                .AsAsyncEnumerable();

                return (IAsyncEnumerable<T>)entity;
            }

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