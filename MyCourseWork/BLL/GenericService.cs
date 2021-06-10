using BLLAbstractions.Interfaces;
using Core;
using Core.DbModels;
using DALAbstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly IGenericRepository<T> _userDalService;

        public GenericService(IGenericRepository<T> userDalService)
        {
            _userDalService = userDalService;
        }

        public Task Add(T entity)
        {
            return _userDalService.Add(entity);
        }

        public Task<int> CountAll()
        {
            return _userDalService.CountAll();
        }

        public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
        {
            return _userDalService.CountWhere(predicate);
        }

        public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _userDalService.FirstOrDefault(predicate);
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return _userDalService.GetAll();
        }

        public Task<T> GetById(int id)
        {
            return _userDalService.GetById(id);
        }

        public Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return _userDalService.GetWhere(predicate);
        }

        public Task Remove(T entity)
        {
            return _userDalService.Remove(entity);
        }

        public Task Update(T entity)
        {
            return _userDalService.Update(entity);
        }
    }
}
