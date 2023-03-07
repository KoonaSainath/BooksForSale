﻿using Sainath.E_Commerce.BooksForSale.DataAccess.BaseClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Customer
{
    public class ManageOrdersData : BaseData
    {
        public ManageOrdersData(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<OrderHeader> GetAllOrders(string userId, bool isUserAdminOrEmployee, string status, string? includeProperties)
        {
            IEnumerable<OrderHeader> orders = new List<OrderHeader>();
            Expression<Func<OrderHeader, bool>> userIdFilter = (order => order.UserId == userId);
            Expression<Func<OrderHeader, bool>> statusFilter;
            if(status.NullCheckTrim().ToLower() == OrderStatus.PAYMENT_STATUS_DELAYED_PAYMENT.ToLower())
            {
                statusFilter = (order => order.PaymentStatus == OrderStatus.PAYMENT_STATUS_DELAYED_PAYMENT);
            }
            else
            {
                statusFilter = (order => order.OrderStatus == status);
            }
            Expression<Func<OrderHeader, bool>> userIdAndStatusFilter;
            if(status.NullCheckTrim().ToLower() == OrderStatus.PAYMENT_STATUS_DELAYED_PAYMENT.ToLower())
            {
                userIdAndStatusFilter = (order => order.UserId == userId && order.PaymentStatus == OrderStatus.PAYMENT_STATUS_DELAYED_PAYMENT);
            }
            else
            {
                userIdAndStatusFilter = (order => order.UserId == userId && order.OrderStatus == status);
            }
            if (isUserAdminOrEmployee)
            {
                if(status.NullCheckTrim().ToLower() == GenericConstants.ALL.ToLower())
                {
                    orders = unitOfWork.OrderHeaderRepository.GetAllRecords(includeProperties).OrderByDescending(order => order.OrderHeaderId);
                }
                else
                {
                    orders = unitOfWork.OrderHeaderRepository.GetAllRecords(includeProperties, statusFilter);
                }
            }
            else
            {
                if(status.NullCheckTrim().ToLower() == GenericConstants.ALL.ToLower())
                {
                    orders = unitOfWork.OrderHeaderRepository.GetAllRecords(includeProperties, userIdFilter);
                }
                else
                {
                    orders = unitOfWork.OrderHeaderRepository.GetAllRecords(includeProperties, userIdAndStatusFilter);
                }
            }
            return orders;
        }

        public OrderHeader GetOrder(int orderHeaderId, string includeProperties)
        {
            OrderHeader orderHeader = unitOfWork.OrderHeaderRepository.GetRecordByExpression((order => order.OrderHeaderId == orderHeaderId), includeProperties);
            return orderHeader;
        }

        public IEnumerable<OrderDetails> GetOrderDetails(int orderHeaderId, string includeProperties)
        {
            IEnumerable<OrderDetails> orderDetails = unitOfWork.OrderDetailsRepository.GetAllRecords(includeProperties, (order => order.OrderHeaderId == orderHeaderId)).ToList();
            return orderDetails;
        }
    }
}
