using BulkyBook.BLL.Interfaces;
using BulkyBook.DAL.Context;
using BulkyBook.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T item)
        {
            _dbContext.Set<T>().Add(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        public IReadOnlyList<T> GetAll()
        {
            if( typeof(T) == typeof(Product))
                return (IReadOnlyList<T>) _dbContext.Set<Product>().Include(p => p.Category).Include(p => p.CoverType).ToList();
            if (typeof(T) == typeof(ShoppingCart))
                return (IReadOnlyList<T>)_dbContext.Set<ShoppingCart>().Include( c=> c.Product).ToList();
            return _dbContext.Set<T>().ToList();
        }

        public T GetById(Expression<Func<T,bool>> filter)
            => _dbContext.Set<T>().Where(filter).FirstOrDefault();
    }
}
