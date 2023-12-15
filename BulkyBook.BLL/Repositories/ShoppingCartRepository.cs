using BulkyBook.BLL.Interfaces;
using BulkyBook.DAL.Context;
using BulkyBook.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Repositories
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart> , IShoppingCartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ShoppingCartRepository( ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }

        public void RemoveRange(IEnumerable<ShoppingCart> shoppingCarts)
        {
            _dbContext.ShoppingCarts.RemoveRange(shoppingCarts);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        

        public void Update(ShoppingCart shoppingCart)
        {
            _dbContext.ShoppingCarts.Update(shoppingCart);
        }


    }
}
