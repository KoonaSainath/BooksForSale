using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories.Customer
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        public void UpdateShoppingCart(ShoppingCart shoppingCart);
    }
}
