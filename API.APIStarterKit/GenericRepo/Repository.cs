using Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Core.Data.Context;

namespace API.APIStarterKit.GenericRepo
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.Where(x => x.Cancelled == false).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Int64 id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity != null)
            {
                return entity;
            }
            return null;
        }
        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public void Add(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            _entities.Add(entity);
        }
        public void Update(T entity, T entityCurrent)
        {
            entity.ModifiedDate = DateTime.Now;
            _entities.Entry(entityCurrent).CurrentValues.SetValues(entity);
        }
        public async Task<T> Delete(Int64 id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                return entity;
            }
            return null;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
    }
}
