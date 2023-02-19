﻿using Sainath.E_Commerce.BooksForSale.DataAccess.BaseClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
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

        public ShoppingCart GetShoppingCart(int bookId, string userId, int shoppingCartId)
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            string includeProperties = "Book,BooksForSaleUser,Book.Category,Book.CoverType";
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
    }
}
