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
    public class ShoppingCartData : BaseData
    {
        public ShoppingCartData(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public IEnumerable<ShoppingCart> GetAllShoppingCarts(string userId, string includeProperties)
        {
            Expression<Func<ShoppingCart, bool>> expression = (cart => cart.Id == userId);
            return unitOfWork.ShoppingCartRepository.GetAllRecords(includeProperties, expression).OrderByDescending(cart => cart.ShoppingCartId).AsEnumerable();
        }

        public ShoppingCart GetShoppingCart(int bookId, string userId, int shoppingCartId, string includeProperties)
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            if(bookId != 0 && userId != null)
            {
                shoppingCart = unitOfWork.ShoppingCartRepository.GetRecordByExpression((cart => cart.BookId == bookId && cart.Id == userId), includeProperties);
            }
            else if (shoppingCartId != 0)
            {
                shoppingCart = unitOfWork.ShoppingCartRepository.GetRecordByExpression((cart => cart.ShoppingCartId == shoppingCartId), includeProperties);
            }

            if(shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
            }
            return shoppingCart;
        }

        public void InsertShoppingCart(ShoppingCart shoppingCart)
        {
            unitOfWork.ShoppingCartRepository.InsertRecord(shoppingCart);
            unitOfWork.Save();
        }

        public void RemoveShoppingCart(ShoppingCart shoppingCart)
        {
            unitOfWork.ShoppingCartRepository.RemoveRecord(shoppingCart);
            unitOfWork.Save();
        }

        public void RemoveShoppingCarts(IEnumerable<ShoppingCart> shoppingCarts)
        {
            unitOfWork.ShoppingCartRepository.RemoveRecords(shoppingCarts);
            unitOfWork.Save();
        }

        public void UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            unitOfWork.ShoppingCartRepository.UpdateShoppingCart(shoppingCart);
            unitOfWork.Save();
        }

        public void IncrementBookCountInShoppingCart(ShoppingCart shoppingCart)
        {
            unitOfWork.ShoppingCartRepository.IncrementBookCountInShoppingCart(shoppingCart);
            unitOfWork.Save();
        }

        public void DecrementBookCountInShoppingCart(ShoppingCart shoppingCart)
        {
            unitOfWork.ShoppingCartRepository.DecrementBookCountInShoppingCart(shoppingCart);
            unitOfWork.Save();
        }

        public OrderHeader InsertOrderHeader(OrderHeader orderHeader)
        {
            unitOfWork.OrderHeaderRepository.InsertRecord(orderHeader);
            unitOfWork.Save();
            return orderHeader;
        }

        public void UpdateOrderHeader(OrderHeader orderHeader)
        {
            unitOfWork.OrderHeaderRepository.Update(orderHeader);
            unitOfWork.Save();
        }

        public void UpdateOrderHeaderStatus(int orderHeaderId, string orderStatus, string? paymentStatus)
        {
            unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeaderId, orderStatus, paymentStatus);
            unitOfWork.Save();
        }

        public void InsertOrderDetails(OrderDetails orderDetails)
        {
            unitOfWork.OrderDetailsRepository.InsertRecord(orderDetails);
            unitOfWork.Save();
        }

        public void UpdateStripeStatus(int orderHeaderId, string stripeSessionId, string stripePaymentIntentId)
        {
            unitOfWork.OrderHeaderRepository.UpdateStripeStatus(orderHeaderId, stripeSessionId, stripePaymentIntentId);
            unitOfWork.Save();
        }

        public OrderHeader GetOrderHeader(int orderHeaderId, string includeProperties)
        {
            OrderHeader orderHeader = unitOfWork.OrderHeaderRepository.GetRecordByExpression((orderHeader => orderHeader.OrderHeaderId == orderHeaderId), includeProperties: includeProperties);
            return orderHeader;
        }
    }
}
