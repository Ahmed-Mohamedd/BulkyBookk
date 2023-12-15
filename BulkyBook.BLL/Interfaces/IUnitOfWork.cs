using BulkyBook.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Categories{ get; }
        ICoverTypeRepository CoverTypes { get; }   
        IProductRepository Products { get; }
        IShoppingCartRepository ShoppingCarts { get; }
        IApplicationUserRepository ApplicationUsers { get; }
        IOrderHeaderRepository OrderHeaders { get; }
        IOrderDetailRepository OrderDetails { get; }

        int Complete();
    }
}
