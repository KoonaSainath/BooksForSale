using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Customer;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;

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

        [Route(template: "GET/GetAllShoppingCarts/{userId?}/{includeProperties?}", Name = "GetAllShoppingCarts")]
        [HttpGet]
        public IActionResult GetAllShoppingCarts(string? userId = null, string? includeProperties = null)
        {
            IEnumerable<ShoppingCart> shoppingCarts = shoppingCartDomain.GetAllShoppingCarts(userId, includeProperties);
            return Ok(shoppingCarts);
        }

        [Route(template: "GET/GetShoppingCart/{bookId?}/{userId?}/{shoppingCartId?}/{includleProperties?}", Name = "GetShoppingCart")]
        [HttpGet]
        public IActionResult GetShoppingCart(int? bookId = 0, string? userId = null, int? shoppingCartId = 0, string? includeProperties = null)
        {
            ShoppingCart shoppingCart = shoppingCartDomain.GetShoppingCart((int)bookId, userId, (int)shoppingCartId, includeProperties);
            return Ok(shoppingCart);
        }

        [Route(template: "POST/InsertShoppingCart", Name = "InsertShoppingCart")]
        [HttpPost]
        public IActionResult InsertShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartDomain.InsertShoppingCart(shoppingCart);
            return Ok("Shopping cart is inserted successfully!");
        }

        [Route(template: "DELETE/RemoveShoppingCart", Name = "RemoveShoppingCart")]
        [HttpPost]
        public IActionResult RemoveShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartDomain.RemoveShoppingCart(shoppingCart);
            return Ok("Shopping cart is removed successfully!");
        }

        [Route(template: "DELETE/RemoveShoppingCarts", Name = "RemoveShoppingCarts")]
        [HttpPost]
        public IActionResult RemoveShoppingCarts(IEnumerable<ShoppingCart> shoppingCarts)
        {
            shoppingCartDomain.RemoveShoppingCarts(shoppingCarts);
            return Ok("Shopping carts are removed successfully!");
        }

        [HttpPut]
        [Route(template: "PUT/UpdateShoppingCart", Name = "UpdateShoppingCart")]
        public IActionResult UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartDomain.UpdateShoppingCart(shoppingCart);
            return Ok("Shopping cart is updated successfully");
        }

        [HttpPut]
        [Route(template: "PUT/IncrementBookCountInShoppingCart", Name = "IncrementBookCountInShoppingCart")]
        public IActionResult IncrementBookCountInShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartDomain.IncrementBookCountInShoppingCart(shoppingCart);
            return Ok();
        }

        [HttpPut]
        [Route(template: "PUT/DecrementBookCountInShoppingCart", Name = "DecrementBookCountInShoppingCart")]
        public IActionResult DecrementBookCountInShoppingCart(ShoppingCart shoppingCart)
        {
            shoppingCartDomain.DecrementBookCountInShoppingCart(shoppingCart);
            return Ok();
        }

        [HttpPost]
        [Route(template: "POST/InsertOrderHeader", Name = "InsertOrderHeader")]
        public IActionResult InsertOrderHeader(OrderHeader orderHeader)
        {
            OrderHeader insertedOrderHeader = shoppingCartDomain.InsertOrderHeader(orderHeader);
            return Ok(insertedOrderHeader);
        }

        [HttpPut]
        [Route(template: "PUT/UpdateOrderHeader", Name = "UpdateOrderHeader")]
        public IActionResult UpdateOrderHeader(OrderHeader orderHeader)
        {
            shoppingCartDomain.UpdateOrderHeader(orderHeader);
            return Ok("Order header updated successfully");
        }

        [HttpPut]
        [Route(template: "PUT/UpdateOrderHeaderStatus/{orderHeaderId}/{orderStatus}/{paymentStatus?}", Name = "UpdateOrderHeaderStatus")]
        public IActionResult UpdateOrderHeaderStatus(OrderHeader orderHeader, int orderHeaderId, string orderStatus, string? paymentStatus = null)
        {
            shoppingCartDomain.UpdateOrderHeaderStatus(orderHeaderId, orderStatus, paymentStatus);
            return Ok("Order header status updated successfully");
        }

        [HttpPost]
        [Route(template: "POST/InsertOrderDetails", Name = "InsertOrderDetails")]
        public IActionResult InsertOrderDetails(OrderDetails orderDetails)
        {
            shoppingCartDomain.InsertOrderDetails(orderDetails);
            return Ok("Order details inserted successfully");
        }

        [HttpPut]
        [Route(template: "PUT/UpdateStripeStatus/{orderHeaderId}/{stripeSessionId}/{stripePaymentIntentId}", Name = "UpdateStripeStatus")]
        public IActionResult UpdateStripeStatus(OrderHeader orderHeader, int orderHeaderId, string stripeSessionId, string stripePaymentIntentId)
        {
            shoppingCartDomain.UpdateStripeStatus(orderHeaderId, stripeSessionId, stripePaymentIntentId);
            return Ok("Stripe status updated successfully");
        }

        [HttpGet]
        [Route(template: "GET/GetOrderHeader/{orderHeaderId}", Name = "GetOrderHeader")]
        public IActionResult GetOrderHeader(int orderHeaderId)
        {
            OrderHeader orderHeader = shoppingCartDomain.GetOrderHeader(orderHeaderId);
            return Ok(orderHeader);
        }
    }
}
