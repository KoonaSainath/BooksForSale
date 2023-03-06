using Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Customer;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
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
        public IEnumerable<OrderHeader> GetAllOrders(string userId, bool IsUserAdminOrEmployee, string? includeProperties)
        {
            return manageOrdersData.GetAllOrders(userId, IsUserAdminOrEmployee, includeProperties);
        }
    }
}
