using Sainath.E_Commerce.BooksForSale.DataAccess.BaseClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
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

        public IEnumerable<OrderHeader> GetAllOrders(string userId, bool isUserAdminOrEmployee, string? includeProperties)
        {
            IEnumerable<OrderHeader> orders = new List<OrderHeader>();
            if (isUserAdminOrEmployee)
            {
                orders = unitOfWork.OrderHeaderRepository.GetAllRecords(includeProperties).OrderByDescending(order => order.OrderHeaderId);
            }
            else
            {
                Expression<Func<OrderHeader, bool>> expression = (order => order.UserId == userId);
                orders = unitOfWork.OrderHeaderRepository.GetAllRecords(includeProperties, expression);
            }
            return orders;
        }
    }
}
