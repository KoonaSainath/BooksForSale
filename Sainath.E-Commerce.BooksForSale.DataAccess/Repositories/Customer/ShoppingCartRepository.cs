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
        private readonly BooksForSaleDbContext dbContext;
        public ShoppingCartRepository(BooksForSaleDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            ShoppingCart shoppingCartFromDb = dbContext.ShoppingCarts.Find(shoppingCart.ShoppingCartId);
            if(shoppingCartFromDb != null)
            {
                shoppingCartFromDb.CartItemCount = shoppingCart.CartItemCount;
            }
        }
    }
}
