using BulkyBook.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Interfaces
{
    public  interface IOrderDetailRepository:IGenericRepository<OrderDetail>
    {
        

        void Update(OrderDetail OrderDetail);

        

        void Save();

    }
}
