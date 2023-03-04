using Microsoft.Identity.Client;
using Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories.Customer;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.Repositories.Customer
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly BooksForSaleDbContext dbContext;
        public OrderHeaderRepository(BooksForSaleDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Update(OrderHeader orderHeader)
        {
            dbContext.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int orderHeaderId, string orderStatus, string? paymentStatus = null)
        {
            OrderHeader orderHeader = dbContext.OrderHeaders.Find(orderHeaderId);
            if (orderHeader != null)
            {
                orderHeader.OrderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    orderHeader.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripeStatus(int orderHeaderId, string stripeSessionId, string stripePaymentIntentId)
        {
            OrderHeader orderHeader = dbContext.OrderHeaders.Find(orderHeaderId);
            orderHeader.StripeSessionId = stripeSessionId;
            orderHeader.StripePaymentIntentId = stripePaymentIntentId;
        }
    }
}
