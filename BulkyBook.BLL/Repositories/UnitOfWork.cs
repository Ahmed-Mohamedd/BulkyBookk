using BulkyBook.BLL.Interfaces;
using BulkyBook.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public ICategoryRepository Categories { get; private set; }
        public ICoverTypeRepository CoverTypes { get; private set; }
        public IProductRepository Products { get; private set; }
        public IShoppingCartRepository ShoppingCarts { get; private set;}
        public IApplicationUserRepository ApplicationUsers { get; private set;}
        public IOrderHeaderRepository OrderHeaders { get; private set;}
        public IOrderDetailRepository OrderDetails { get; private set;}

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
            Categories = new CategoryRepository(_dbContext);
            CoverTypes = new CoverTypeRepository(_dbContext);
            Products = new ProductRepository(_dbContext);
            ShoppingCarts = new ShoppingCartRepository(_dbContext);
            ApplicationUsers = new ApplicationUserRepository(_dbContext);
            OrderHeaders = new OrderHeaderRepository(_dbContext);
            OrderDetails = new OrderDetailRepository(_dbContext);
        }

        public void Dispose()
        => _dbContext.Dispose();

        public int Complete()
         => _dbContext.SaveChanges();   
    }
}
