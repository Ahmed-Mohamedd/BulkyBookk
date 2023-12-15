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
    public class OrderHeaderRepository : GenericRepository<OrderHeader> ,  IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderHeaderRepository( ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(OrderHeader OrderHeader)
        {
            _dbContext.OrderHeaders.Update(OrderHeader);
        }

		public void UpdateStatus(int id, string OrderStatus, string? PaymentStatus = null)
		{
			var orderHeaderFromDb = _dbContext.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if(orderHeaderFromDb !=  null)
            {
                orderHeaderFromDb.OrderStatus = OrderStatus;
                if(PaymentStatus != null)
                {
                    orderHeaderFromDb.PaymentStatus = PaymentStatus;
                }
            }
		}

        public void UpdateStripePaymentId(int id, string SessionId, string PaymentIntentId)
        {
            var orderHeaderFromDb = _dbContext.OrderHeaders.FirstOrDefault(o => o.Id == id);

            orderHeaderFromDb.SessionId = SessionId;
            orderHeaderFromDb.PaymentIntentId = PaymentIntentId;
        }
    }
}
