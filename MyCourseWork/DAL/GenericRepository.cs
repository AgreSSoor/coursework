using Core;
using Core.DbModels;
using DALAbstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext Context;

        public GenericRepository(DbContext context)
        {
            Context = context;
        }

        #region Public Methods

        public Task<TEntity> GetById(int id) => Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

        public Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
            => Context.Set<TEntity>().FirstOrDefaultAsync(predicate);

        public async Task Add(TEntity entity)
        {
            // await Context.AddAsync(entity);
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public Task Update(TEntity entity)
        {
            // In case AsNoTracking is used
            Context.Entry(entity).State = EntityState.Modified;
            return Context.SaveChangesAsync();
        }

        public Task Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public Task<int> CountAll() => Context.Set<TEntity>().CountAsync();

        public Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate)
            => Context.Set<TEntity>().CountAsync(predicate);

        #endregion
    }
}
