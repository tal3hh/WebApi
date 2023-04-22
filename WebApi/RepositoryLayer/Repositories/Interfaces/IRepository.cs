using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable GetQueryable();
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true);
        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true);
        Task<T?> FindAsync(int id);
        Task<List<T>> FilterAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true);
        Task<List<T>> GetAllOrderByAsync(Expression<Func<T, int>> exp, bool AscOrDesc = true);

    }
}
