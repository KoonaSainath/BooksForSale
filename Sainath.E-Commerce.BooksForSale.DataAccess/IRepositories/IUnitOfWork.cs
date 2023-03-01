using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories.Customer;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        ICoverTypeRepository CoverTypeRepository { get; }
        IBookRepository BookRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IBooksForSaleUserRepository BooksForSaleUserRepository { get; }
        IOrderHeaderRepository OrderHeaderRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }
        void Save();
    }
}
