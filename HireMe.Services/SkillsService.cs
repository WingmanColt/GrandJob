using HireMe.Core.Helpers;
using HireMe.Data.Repository.Interfaces;
using HireMe.Entities.Models;
using HireMe.Mapping.Utility;
using HireMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Services
{
    public class SkillsService : ISkillsService
    {
        private readonly IRepository<Skills> skillsRepository;

        public SkillsService(IRepository<Skills> skillsRepository)      
        {
            this.skillsRepository = skillsRepository;
        }

        public IAsyncEnumerable<T> GetAll<T>(string SkillId, bool isMapped)
        {
            string[] words = SkillId?.Split(',');
            if (words is null)
                return null;


            if (isMapped)
            {
                var entity = GetAllAsNoTracking()
                .Where(x => ((IList)words).Contains(x.Title))
                .To<T>()
                .AsAsyncEnumerable();

                return entity;
            }
            else
            {
                var entity = GetAllAsNoTracking()
                .Where(x => ((IList)words).Contains(x.Title))
                .AsAsyncEnumerable();

                return (IAsyncEnumerable<T>)entity;
            }

        }
        public IAsyncEnumerable<T> GetAllById<T>(string SkillId, bool isMapped)
        {
            string[] words = SkillId?.Split(',');
            if (words is null)
                return null;


            if (isMapped)
            {
                var entity = GetAllAsNoTracking()
                .Where(x => ((IList)words).Contains(x.Title))
                .To<T>()
                .AsAsyncEnumerable();

                return entity;
            }
            else
            {
                var entity = GetAllAsNoTracking()
                .Where(x => ((IList)words).Contains(x.Title))
                .AsAsyncEnumerable();

                return (IAsyncEnumerable<T>)entity;
            }

        }
        private IQueryable<Skills> GetAllAsNoTracking()
        {
            return skillsRepository.Set().AsNoTracking();
        }

        public async Task<OperationResult> SeedSkills()
        {
            if (await GetAllAsNoTracking().AnyAsync())
                return OperationResult.FailureResult("Skills already exists.");

            string[] lines = await System.IO.File.ReadAllLinesAsync(@"wwwroot/Skills.txt");

            foreach (string line in lines)
            {
                 var skills = new Skills
                {
                  Title = line
                };
                await skillsRepository.AddRangeAsync(skills);
            }

           var result = await skillsRepository.SaveChangesAsync();
            return result;
        }
    }
}