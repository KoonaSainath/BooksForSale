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

        public IEnumerable<ShoppingCart> GetAllShoppingCarts(string userId, string includeProperties)
        {
            return shoppingCartData.GetAllShoppingCarts(userId, includeProperties);
        }

        public ShoppingCart GetShoppingCart(int bookId, string userId, int shoppingCartId, string includeProperties)
        {
            return shoppingCartData.GetShoppingCart(bookId, userId, shoppingCartId, includeProperties);
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

        public void IncrementBookCountInShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartData.IncrementBookCountInShoppingCart(shoppingCart);
        }

        public void DecrementBookCountInShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartData.DecrementBookCountInShoppingCart(shoppingCart);
        }

        public OrderHeader InsertOrderHeader(OrderHeader orderHeader)
        {
            OrderHeader insertedOrderHeader = shoppingCartData.InsertOrderHeader(orderHeader);
            return insertedOrderHeader;
        }

        public void UpdateOrderHeader(OrderHeader orderHeader)
        {
            shoppingCartData.UpdateOrderHeader(orderHeader);
        }

        public void UpdateOrderHeaderStatus(int orderHeaderId, string orderStatus, string? paymentStatus = null)
        {
            shoppingCartData.UpdateOrderHeaderStatus(orderHeaderId, orderStatus, paymentStatus);
        }

        public void InsertOrderDetails(OrderDetails orderDetails)
        {
            shoppingCartData.InsertOrderDetails(orderDetails);
        }

        public void UpdateStripeStatus(int orderHeaderId, string stripeSessionId, string stripePaymentIntentId)
        {
            shoppingCartData.UpdateStripeStatus(orderHeaderId, stripeSessionId, stripePaymentIntentId);
        }

        public OrderHeader GetOrderHeader(int orderHeaderId, string includeProperties)
        {
            return shoppingCartData.GetOrderHeader(orderHeaderId, includeProperties);
        }
    }
}
