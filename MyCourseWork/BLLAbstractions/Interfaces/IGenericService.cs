using Core;
using Core.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLLAbstractions.Interfaces
{
    public interface IGenericService<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetById(int id);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Remove(TEntity entity);

        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate);
    }
}
