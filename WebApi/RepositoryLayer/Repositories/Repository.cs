using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using RepositoryLayer.Contexts;
using RepositoryLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        readonly DbSet<T> table;
        public Repository(AppDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await table.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllOrderByAsync(Expression<Func<T, int>> exp, bool AscOrDesc = true)
        {
            return AscOrDesc ? await table.AsNoTracking().OrderBy(exp).ToListAsync() : await table.AsNoTracking().OrderByDescending(exp).ToListAsync();
        }

        public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true)
        {
            return AsnoTracking ? await table.AsNoTracking().Where(exp).ToListAsync() : await table.Where(exp).ToListAsync();
        }


        public async Task<T?> FindAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true)
        {
            return AsnoTracking ? await table.AsNoTracking().SingleOrDefaultAsync(exp) : await table.SingleOrDefaultAsync(exp);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> exp, bool AsnoTracking = true)
        {
            return AsnoTracking ? await table.AsNoTracking().FirstOrDefaultAsync(exp) : await table.FirstOrDefaultAsync(exp);
        }

        public IQueryable GetQueryable()
        {
            return table.AsQueryable();
        }

        public async Task CreateAsync(T entity)
        {
            if (entity is null) throw new ArgumentNullException();

            await table.AddAsync(entity);
            
        }

        public void Delete(T entity)
        {
            if (entity is null) throw new ArgumentNullException();

            table.Remove(entity);
        }

        public void Update(T entity)
        {
            if (entity is null) throw new ArgumentNullException();

            table.Update(entity);
        }
    }
}
