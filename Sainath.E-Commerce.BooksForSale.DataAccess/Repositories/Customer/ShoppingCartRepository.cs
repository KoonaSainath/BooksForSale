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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(BooksForSaleDbContext dbContext) : base(dbContext)
        {

        }
    }
}
