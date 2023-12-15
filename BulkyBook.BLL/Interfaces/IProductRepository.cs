using BulkyBook.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        void Update(Product product);

        Product GetByIdSpecification(Expression<Func<Product, bool>> filter);

        void Save();
    }
}
