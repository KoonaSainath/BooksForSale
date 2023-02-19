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
    public class ShoppingCartDomain
    {
        private readonly ShoppingCartData shoppingCartData;
        public ShoppingCartDomain(IUnitOfWork unitOfWork)
        {
            shoppingCartData = new ShoppingCartData(unitOfWork);
        }

        public IEnumerable<ShoppingCart> GetAllShoppingCarts()
        {
            return shoppingCartData.GetAllShoppingCarts();
        }

        public ShoppingCart GetShoppingCart(int bookId, string userId, int shoppingCartId)
        {
            return shoppingCartData.GetShoppingCart(bookId, userId, shoppingCartId);
        }

        public void InsertShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartData.InsertShoppingCart(shoppingCart);
        }

        public void RemoveShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartData.RemoveShoppingCart(shoppingCart);
        }

        public void RemoveShoppingCarts(IEnumerable<ShoppingCart> shoppingCarts)
        {
            shoppingCartData.RemoveShoppingCarts(shoppingCarts);
        }

        public void UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartData.UpdateShoppingCart(shoppingCart);
        }
    }
}
