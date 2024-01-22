using CosmosDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDemo.Repository
{
    public abstract class Repository<T> : IRepository<T> 
                            where T : class, IEntity
    {
        protected readonly DbContext _dbContext;

        //protected DbSet<T> _dbSet;

        protected Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            //_dbSet = _dbContext.Set<T>();
        }


        public virtual async Task<T> GetById(string id)
        {
            return await _dbContext.Set<T>()
                .FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public virtual async Task<T> Create(T entity)
        {
            if (entity.Id is null)
            {
                entity.Id = Guid.NewGuid().ToString();
            }

            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return await GetById(entity.Id);
        }

        public virtual async Task<IList<T>> CreateMany(IEnumerable<T> entities)
        {
            if (entities.Any())
            {
                entities.ToList().ForEach(a => a.Id = Guid.NewGuid().ToString());
            }

            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities.ToList();
        }

        public virtual async Task<T> Update(T entity)
        {
            var entry = _dbContext.Add(entity);
            entry.State = EntityState.Unchanged;
            _dbContext.Set<T>().Update(entity);

            //_dbContext.Attach(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return await GetById(entity.Id);
        }

        public virtual async Task Delete(string id)
        {
            var entity = await GetById(id);

            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
