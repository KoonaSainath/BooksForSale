using Sainath.E_Commerce.BooksForSale.DataAccess.BaseClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Customer
{
    public class ShoppingCartData : BaseData
    {
        public ShoppingCartData(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public IEnumerable<ShoppingCart> GetAllShoppingCarts()
        {
            return unitOfWork.ShoppingCartRepository.GetAllRecords().OrderByDescending(cart => cart.ShoppingCartId).AsEnumerable();
        }

        public ShoppingCart GetShoppingCart(int shoppingCartId)
        {
            return unitOfWork.ShoppingCartRepository.GetRecord(shoppingCartId);
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
    }
}
