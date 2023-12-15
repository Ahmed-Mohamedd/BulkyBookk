using BulkyBook.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Interfaces
{
    public  interface IShoppingCartRepository:IGenericRepository<ShoppingCart>
    {
        //IReadOnlyList<Category> GetAllCategories();

        //Category GetById(int? id);

        //int Add(Category category);
        //int Update(Category category);

        //int Delete(Category category);

        void Update(ShoppingCart shoppingCart);

        int IncrementCount(ShoppingCart shoppingCart , int count);
        int DecrementCount(ShoppingCart shoppingCart , int count);

        void RemoveRange(IEnumerable<ShoppingCart> shoppingCarts);

        void Save();

    }
}
