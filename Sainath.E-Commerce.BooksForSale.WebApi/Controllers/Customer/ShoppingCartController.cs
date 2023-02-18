using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Customer;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models;

namespace Sainath.E_Commerce.BooksForSale.WebApi.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartDomain shoppingCartDomain;
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            shoppingCartDomain = new ShoppingCartDomain(unitOfWork);
        }

        [Route(template: "GET/GetAllShoppingCarts", Name = "GetAllShoppingCarts")]
        public IActionResult GetAllShoppingCarts()
        {
            IEnumerable<ShoppingCart> shoppingCarts = shoppingCartDomain.GetAllShoppingCarts();
            return Ok(shoppingCarts);
        }

        [Route(template: "GET/GetShoppingCart/{shoppingCartId}", Name = "GetShoppingCart")]
        public IActionResult GetShoppingCart(int shoppingCartId)
        {
            ShoppingCart shoppingCart = shoppingCartDomain.GetShoppingCart(shoppingCartId);
            return Ok(shoppingCart);
        }

        [Route(template: "POST/InsertShoppingCart", Name = "InsertShoppingCart")]
        public IActionResult InsertShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartDomain.InsertShoppingCart(shoppingCart);
            return Ok("Shopping cart is inserted successfully!");
        }

        [Route(template: "DELETE/RemoveShoppingCart", Name = "RemoveShoppingCart")]
        public IActionResult RemoveShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartDomain.RemoveShoppingCart(shoppingCart);
            return Ok("Shopping cart is removed successfully!");
        }

        [Route(template: "DELETE/RemoveShoppingCarts", Name = "RemoveShoppingCarts")]
        public IActionResult RemoveShoppingCarts(IEnumerable<ShoppingCart> shoppingCarts)
        {
            shoppingCartDomain.RemoveShoppingCarts(shoppingCarts);
            return Ok("Shopping carts are removed successfully!");
        }
    }
}
