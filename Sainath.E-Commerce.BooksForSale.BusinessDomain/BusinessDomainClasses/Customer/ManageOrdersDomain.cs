using Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Customer;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Customer
{
    public class ManageOrdersDomain
    {
        private readonly ManageOrdersData manageOrdersData;
        public ManageOrdersDomain(IUnitOfWork unitOfWork)
        {
            manageOrdersData = new ManageOrdersData(unitOfWork);
        }
        public IEnumerable<OrderHeader> GetAllOrders(string userId, bool isUserAdminOrEmployee, string status, string? includeProperties)
        {
            return manageOrdersData.GetAllOrders(userId, isUserAdminOrEmployee, status, includeProperties);
        }
        public OrderHeader GetOrder(int orderHeaderId, string includeProperties)
        {
            return manageOrdersData.GetOrder(orderHeaderId, includeProperties);
        }
        public IEnumerable<OrderDetails> GetOrderDetails(int orderHeaderId, string includeProperties)
        {
            return manageOrdersData.GetOrderDetails(orderHeaderId, includeProperties);
        }
        public void UpdateOrder(OrderHeader orderHeader)
        {
            manageOrdersData.UpdateOrder(orderHeader);
        }
        public void StartOrderProcessing(int orderHeaderId)
        {
            manageOrdersData.StartProcessingOrder(orderHeaderId);
        }
        public void ShipOrder(OrderHeader orderHeader)
        {
            OrderHeader orderHeaderFromDb = manageOrdersData.GetOrder(orderHeader.OrderHeaderId, null);
            if(orderHeaderFromDb != null)
            {
                orderHeaderFromDb.OrderStatus = orderHeader.OrderStatus;
                orderHeaderFromDb.ShippingDate = orderHeader.ShippingDate;
                if(orderHeader.PaymentDueDate != null)
                {
                    orderHeaderFromDb.PaymentDueDate = orderHeader.PaymentDueDate;
                }
                if(orderHeader.Carrier != null)
                {
                    orderHeaderFromDb.Carrier = orderHeader.Carrier;
                }
                if (orderHeader.TrackingNumber != null)
                {
                    orderHeaderFromDb.TrackingNumber = orderHeader.TrackingNumber;
                }
                manageOrdersData.ShipOrder(orderHeaderFromDb);
            }
        }
        public void CancelOrder(int orderHeaderId, bool isRefundProcessed)
        {
            manageOrdersData.CancelOrder(orderHeaderId, isRefundProcessed);
        }
    }
}
