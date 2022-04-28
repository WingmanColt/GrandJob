namespace HireMe.Data.Repository
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using HireMe.Data.Repository.Interfaces;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using HireMe.Core.Helpers;

    public class DbBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public DbBaseRepository(BaseDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.dbSet = this.context.Set<TEntity>();
        }

        protected DbSet<TEntity> dbSet { get; set; }
        protected BaseDbContext context { get; set; }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{entity} entity must not be null");
            }

            try
            {
                await this.dbSet.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be added: {ex.Message}");
            }
        }

        public async Task<TEntity> AddRangeAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{entity} entity must not be null");
            }

            try
            {
                await this.dbSet.AddRangeAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be add ranged: {ex.Message}");
            }
        }
        public IQueryable<TEntity> Set()
        {
            try
            {
                return this.dbSet;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public TEntity Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{entity} entity must not be null");
            }

            try
            {
               this.dbSet.Remove(entity);
               return entity;
            }

            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be deleted: {ex.Message}");
            }
        }

        public IQueryable<TEntity> DeleteRange(IQueryable<TEntity> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{entity} entity must not be null");
            }

            try
            {
                this.dbSet.RemoveRange(entity);
                return entity;
            }

            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be deleted: {ex.Message}");
            }
        }
        public TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                var entry = this.context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    this.dbSet.Attach(entity);
                }

                entry.State = EntityState.Modified;
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public async Task<OperationResult> SaveChangesAsync()
        {
            var success = await this.context.SaveChangesAsync() > 0;
            return success ? OperationResult.SuccessResult(null) : OperationResult.FailureResult("Changes saving failure!");
        }
        public OperationResult SaveChanges()
        {
            var success = this.context.SaveChanges() > 0;
            return success ? OperationResult.SuccessResult(null) : OperationResult.FailureResult("Changes saving failure!");
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var ent = await this.dbSet.FindAsync(id);
            return ent;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context?.Dispose();
            }
        }
    }
}
