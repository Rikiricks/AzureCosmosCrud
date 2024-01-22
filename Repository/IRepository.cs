using CosmosDemo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDemo.Repository
{
    public interface IRepository<T> where T: class, IEntity
    {
        public Task<T> GetById(string id);

        public Task<IEnumerable<T>> GetAll();

        public Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression);

        public Task<T> Create(T entity);

        public Task<IList<T>> CreateMany(IEnumerable<T> entity);

        public Task<T> Update(T entity);

        public Task Delete(string id);

    }
}
