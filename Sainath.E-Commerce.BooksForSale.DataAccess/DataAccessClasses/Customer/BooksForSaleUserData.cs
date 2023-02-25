using Sainath.E_Commerce.BooksForSale.DataAccess.BaseClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Customer
{
    public class BooksForSaleUserData : BaseData
    {
        public BooksForSaleUserData(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public BooksForSaleUser GetUser(string userId)
        {
            return unitOfWork.BooksForSaleUserRepository.GetRecordByExpression(user => user.Id == userId);
        }
    }
}
