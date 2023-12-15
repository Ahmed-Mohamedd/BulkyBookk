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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext=dbContext;
        }

        public Product GetByIdSpecification(Expression<Func<Product, bool>> filter)
            => _dbContext.Products.Where(filter).Include(p => p.Category).Include(p => p.CoverType).FirstOrDefault();
        

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Product product)
        {
            _dbContext.Update(product);
        }
    }
}
