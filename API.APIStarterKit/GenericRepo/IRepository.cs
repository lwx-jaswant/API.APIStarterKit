using Core.Data.Models;
using System.Linq.Expressions;

namespace API.APIStarterKit.GenericRepo
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Int64 id);
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity, T entityCurrent);
        Task<T> Delete(Int64 id);
        Task<bool> SaveChangesAsync();
    }
}
