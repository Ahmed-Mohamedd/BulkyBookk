using BulkyBook.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Interfaces
{
    public  interface IOrderHeaderRepository:IGenericRepository<OrderHeader>
    {
        

        void Update(OrderHeader OrderHeader);

        void UpdateStatus(int id  , string OrderStatus , string? PaymentStatus = null);
        void UpdateStripePaymentId(int id  , string SessionId , string PaymentIntentId );

        void Save();

    }
}
