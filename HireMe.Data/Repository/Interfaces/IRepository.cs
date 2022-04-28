using HireMe.Core.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace HireMe.Data.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Set();

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> AddRangeAsync(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity Delete(TEntity entity);

        IQueryable<TEntity> DeleteRange(IQueryable<TEntity> entity);

        Task<OperationResult> SaveChangesAsync();

        OperationResult SaveChanges();

        Task<TEntity> GetByIdAsync(int id);

        void Dispose();
    }
}
