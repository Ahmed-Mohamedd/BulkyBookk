using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Interfaces
{
    public  interface IGenericRepository<T> where T : class
    {
        IReadOnlyList<T> GetAll( );
        

        void Add(T item);

        void Delete(T item);

        T GetById(Expression<Func<T, bool>> filter);
    }
}
