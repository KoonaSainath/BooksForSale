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
    public class BooksForSaleUserDomain
    {
        private readonly BooksForSaleUserData booksForSaleUserData;
        public BooksForSaleUserDomain(IUnitOfWork unitOfWork)
        {
            booksForSaleUserData = new BooksForSaleUserData(unitOfWork);
        }
        public BooksForSaleUser GetUser(string userId)
        {
            return booksForSaleUserData.GetUser(userId); 
        }
    }
}
