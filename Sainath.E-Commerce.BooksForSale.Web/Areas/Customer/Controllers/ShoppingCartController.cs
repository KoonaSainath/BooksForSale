using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels.Customer;
using Sainath.E_Commerce.BooksForSale.Utility;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Utility.Extensions;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Sainath.E_Commerce.BooksForSale.Web.HelperClasses;
using Stripe.Checkout;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IBooksForSaleConfiguration configuration;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public ShoppingCartController(IBooksForSaleConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ShoppingCartVM shoppingCartVM = new ShoppingCartVM();
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;
            string includeProperties = "Book,BooksForSaleUser,Book.Category,Book.CoverType";
            string requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{userId}/{includeProperties}";
            InvokeApi<IEnumerable<ShoppingCart>> invokeApi = new InvokeApi<IEnumerable<ShoppingCart>>(configuration);
            ApiVM<IEnumerable<ShoppingCart>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<ShoppingCart> shoppingCarts = apiVm.TObject;
            shoppingCartVM.OrderHeader = new OrderHeader();
            shoppingCartVM.OrderHeader.TotalOrderAmount = 0;
            foreach (ShoppingCart cart in shoppingCarts)
            {
                cart.Price = CalculateFinalPrice(cart.CartItemCount, cart.Book.Price, cart.Book.Price50, cart.Book.Price100);
                shoppingCartVM.OrderHeader.TotalOrderAmount += (double)(cart.Price * cart.CartItemCount);
            }
            shoppingCartVM.ShoppingCarts = shoppingCarts;
            return View(shoppingCartVM);
        }

        [HttpGet]
        public async Task<IActionResult> IncrementBookCountInShoppingCart(int shoppingCartId)
        {
            string includeProperties = "Book,BooksForSaleUser,Book.Category,Book.CoverType";
            string requestUrl = $"api/ShoppingCart/GET/GetShoppingCart/0/0/{shoppingCartId}/{includeProperties}";
            InvokeApi<ShoppingCart> invokeApi = new InvokeApi<ShoppingCart>(configuration);
            ApiVM<ShoppingCart> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            ShoppingCart shoppingCart = apiVm.TObject;
            if (shoppingCart != null && shoppingCart.CartItemCount < 200)
            {
                requestUrl = $"api/ShoppingCart/PUT/IncrementBookCountInShoppingCart";
                ApiVM<ShoppingCart> apiVmPut = await invokeApi.Invoke(requestUrl, HttpMethod.Put, shoppingCart);
                HttpResponseMessage response = apiVmPut.Response;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DecrementBookCountInShoppingCart(int shoppingCartId)
        {
            string includeProperties = "Book,BooksForSaleUser,Book.Category,Book.CoverType";
            string requestUrl = $"api/ShoppingCart/GET/GetshoppingCart/0/0/{shoppingCartId}/{includeProperties}";
            InvokeApi<ShoppingCart> invokeApi = new InvokeApi<ShoppingCart>(configuration);
            ApiVM<ShoppingCart> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            ShoppingCart shoppingCart = apiVm.TObject;
            if (shoppingCart != null && shoppingCart.CartItemCount > 1)
            {
                requestUrl = "api/ShoppingCart/PUT/DecrementBookCountInShoppingCart";
                ApiVM<ShoppingCart> apiVmPut = await invokeApi.Invoke(requestUrl, HttpMethod.Put, shoppingCart);
                HttpResponseMessage response = apiVmPut.Response;
            }
            else if (shoppingCart != null && shoppingCart.CartItemCount <= 1)
            {
                requestUrl = "api/ShoppingCart/DELETE/RemoveShoppingCart";
                ApiVM<ShoppingCart> apiVmDelete = await invokeApi.Invoke(requestUrl, HttpMethod.Delete, shoppingCart);
                HttpResponseMessage response = apiVmDelete.Response;
                int cartCount = GetCartCount((ClaimsIdentity)User.Identity).Result;
                HttpContext.Session.SetInt32(GenericConstants.CartCountKey, cartCount);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveBookFromShoppingCart(int shoppingCartId)
        {
            string includeProperties = "Book,BooksForSaleUser,Book.Category,Book.CoverType";
            string requestUrl = $"api/ShoppingCart/GET/GetShoppingCart/0/0/{shoppingCartId}/{includeProperties}";
            InvokeApi<ShoppingCart> invokeApi = new InvokeApi<ShoppingCart>(configuration);
            ApiVM<ShoppingCart> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            ShoppingCart shoppingCart = apiVm.TObject;
            if (shoppingCart != null)
            {
                requestUrl = "api/ShoppingCart/DELETE/RemoveShoppingCart";
                ApiVM<ShoppingCart> apiVmDelete = await invokeApi.Invoke(requestUrl, HttpMethod.Delete, shoppingCart);
                HttpResponseMessage response = apiVmDelete.Response;
            }
            int cartCount = GetCartCount((ClaimsIdentity) User.Identity).Result;
            HttpContext.Session.SetInt32(GenericConstants.CartCountKey, cartCount);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Summary()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;
            ShoppingCartVM = new ShoppingCartVM();
            ShoppingCartVM.OrderHeader = new OrderHeader();
            ShoppingCartVM.OrderHeader.UserId = userId;
            string includeProperties = "Book,Book.Category,Book.CoverType";
            string requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{userId}/{includeProperties}";
            InvokeApi<IEnumerable<ShoppingCart>> invokeApiShoppingCarts = new InvokeApi<IEnumerable<ShoppingCart>>(configuration);
            ApiVM<IEnumerable<ShoppingCart>> apiVmShoppingCarts = await invokeApiShoppingCarts.Invoke(requestUrl, HttpMethod.Get);
            ShoppingCartVM.ShoppingCarts = apiVmShoppingCarts.TObject;
            ShoppingCartVM.OrderHeader.TotalOrderAmount = 0;
            foreach (ShoppingCart shoppingCart in ShoppingCartVM.ShoppingCarts)
            {
                shoppingCart.Price = CalculateFinalPrice(shoppingCart.CartItemCount, shoppingCart.Book.Price, shoppingCart.Book.Price50, shoppingCart.Book.Price100);
                ShoppingCartVM.OrderHeader.TotalOrderAmount += (double) (shoppingCart.Price * shoppingCart.CartItemCount);
            }
            requestUrl = $"api/BooksForSaleUser/GET/GetUser/{userId}";
            InvokeApi<BooksForSaleUser> invokeApiUser = new InvokeApi<BooksForSaleUser>(configuration);
            ApiVM<BooksForSaleUser> apiVmUser = await invokeApiUser.Invoke(requestUrl, HttpMethod.Get);
            ShoppingCartVM.OrderHeader.BooksForSaleUser = apiVmUser.TObject;
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.BooksForSaleUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.BooksForSaleUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.BooksForSaleUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.BooksForSaleUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.BooksForSaleUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.BooksForSaleUser.PostalCode;
            ShoppingCartVM.OrderHeader.EstimatedFromDate = DateTime.Now.AddDays(7);
            ShoppingCartVM.OrderHeader.EstimatedToDate = DateTime.Now.AddDays(14);
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> PlaceOrder()
        {
            if (ModelState.IsValid)
            {
                string requestUrl = $"api/BooksForSaleUser/GET/GetUser/{ShoppingCartVM.OrderHeader.UserId}";
                InvokeApi<BooksForSaleUser> invokeApiUser = new InvokeApi<BooksForSaleUser>(configuration);
                ApiVM<BooksForSaleUser> apiVmUser = await invokeApiUser.Invoke(requestUrl, HttpMethod.Get);
                BooksForSaleUser booksForSaleUser = apiVmUser.TObject;
                ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
                if(booksForSaleUser.CompanyId.GetValueOrDefault() == 0)
                {
                    ShoppingCartVM.OrderHeader.OrderStatus = OrderStatus.STATUS_PENDING;
                    ShoppingCartVM.OrderHeader.PaymentStatus = OrderStatus.PAYMENT_STATUS_PENDING;
                }
                else
                {
                    ShoppingCartVM.OrderHeader.OrderStatus = OrderStatus.STATUS_APPROVED;
                    ShoppingCartVM.OrderHeader.PaymentStatus = OrderStatus.PAYMENT_STATUS_DELAYED_PAYMENT;
                }
                string includeProperties = "Book,BooksForSaleUser,Book.Category,Book.CoverType";
                requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{ShoppingCartVM.OrderHeader.UserId}/{includeProperties}";
                InvokeApi<IEnumerable<ShoppingCart>> invokeApiShoppingCarts = new InvokeApi<IEnumerable<ShoppingCart>>(configuration);
                ApiVM<IEnumerable<ShoppingCart>> apiVmShoppingCarts = await invokeApiShoppingCarts.Invoke(requestUrl, HttpMethod.Get);
                ShoppingCartVM.ShoppingCarts = apiVmShoppingCarts.TObject;
                ShoppingCartVM.OrderHeader.TotalOrderAmount = 0;
                foreach (ShoppingCart cart in ShoppingCartVM.ShoppingCarts)
                {
                    cart.Price = CalculateFinalPrice(cart.CartItemCount, cart.Book.Price, cart.Book.Price50, cart.Book.Price100);
                    ShoppingCartVM.OrderHeader.TotalOrderAmount += (double)(cart.Price * cart.CartItemCount);
                }
                requestUrl = "api/ShoppingCart/POST/InsertOrderHeader";
                InvokeApi<OrderHeader> invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
                ApiVM<OrderHeader> apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Post, ShoppingCartVM.OrderHeader);
                HttpResponseMessage response = apiVmOrderHeader.Response;
                OrderHeader orderHeader = response.Content.ReadFromJsonAsync<OrderHeader>().Result;
                if (response.IsSuccessStatusCode)
                {
                    requestUrl = "api/ShoppingCart/POST/InsertOrderDetails";
                    foreach (ShoppingCart cart in ShoppingCartVM.ShoppingCarts)
                    {
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.OrderHeaderId = orderHeader.OrderHeaderId;
                        orderDetails.BookId = cart.BookId;
                        orderDetails.Count = (int)cart.CartItemCount;
                        orderDetails.OrderPrice = cart.Price;

                        InvokeApi<OrderDetails> invokeApiOrderDetails = new InvokeApi<OrderDetails>(configuration);
                        ApiVM<OrderDetails> apiVmOrderDetails = await invokeApiOrderDetails.Invoke(requestUrl, HttpMethod.Post, orderDetails);
                        response = apiVmOrderDetails.Response;
                    }
                }
                if(booksForSaleUser.CompanyId.GetValueOrDefault() == 0)
                {
                    var options = new SessionCreateOptions
                    {
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                        SuccessUrl = configuration.BaseAddressForWebApplication + $"Customer/ShoppingCart/OrderConfirmation?orderHeaderId={orderHeader.OrderHeaderId}",
                        CancelUrl = configuration.BaseAddressForWebApplication + "Customer/ShoppingCart/Index",
                    };

                    foreach (ShoppingCart cart in ShoppingCartVM.ShoppingCarts)
                    {
                        options.LineItems.Add(new SessionLineItemOptions()
                        {
                            Quantity = cart.CartItemCount,
                            PriceData = new SessionLineItemPriceDataOptions()
                            {
                                Currency = "inr",
                                UnitAmount = (long)(cart.Price * 100),
                                ProductData = new SessionLineItemPriceDataProductDataOptions()
                                {
                                    Name = cart.Book.Title
                                }
                            },
                        });
                    }
                    SessionService service = new SessionService();
                    Session session = service.Create(options);
                    session.PaymentIntentId = "Will be updated on order confirmation";
                    requestUrl = $"api/ShoppingCart/PUT/UpdateStripeStatus/{orderHeader.OrderHeaderId}/{session.Id}/{session.PaymentIntentId}";
                    InvokeApi<OrderHeader> invokeApiOrderHeaderPut = new InvokeApi<OrderHeader>(configuration);
                    ApiVM<OrderHeader> apiVmOrderHeaderPut = await invokeApiOrderHeaderPut.Invoke(requestUrl, HttpMethod.Put, ShoppingCartVM.OrderHeader);
                    Response.Headers.Add("Location", session.Url);
                    return new StatusCodeResult(303);
                }
                else
                {
                    return RedirectToAction(nameof(OrderConfirmation), new { orderHeaderId = orderHeader.OrderHeaderId });
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(int orderHeaderId)
        {
            string includeProperties = "BooksForSaleUser";
            string requestUrl = $"api/ShoppingCart/GET/GetOrderHeader/{orderHeaderId}/{includeProperties}";
            InvokeApi<OrderHeader> invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
            ApiVM<OrderHeader> apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Get);
            OrderHeader orderHeader = apiVmOrderHeader.TObject;
            if (orderHeader.PaymentStatus.NullCheckTrim().ToLower() != OrderStatus.PAYMENT_STATUS_DELAYED_PAYMENT.ToLower())
            {
                SessionService sessionService = new SessionService();
                Session session = sessionService.Get(orderHeader.StripeSessionId);
                if (session.PaymentStatus.NullCheckTrim().ToLower() == OrderStatus.PAYMENT_STATUS_PAID.ToLower())
                {
                    requestUrl = $"api/ShoppingCart/PUT/UpdateStripeStatus/{orderHeaderId}/{orderHeader.StripeSessionId}/{session.PaymentIntentId}";
                    InvokeApi<OrderHeader> invokeApiOrderHeaderPut = new InvokeApi<OrderHeader>(configuration);
                    ApiVM<OrderHeader> apiVmOrderHeaderPut = await invokeApiOrderHeaderPut.Invoke(requestUrl, HttpMethod.Put, orderHeader);

                    requestUrl = $"api/ShoppingCart/PUT/UpdateOrderHeaderStatus/{orderHeaderId}/{OrderStatus.STATUS_APPROVED}/{OrderStatus.PAYMENT_STATUS_APPROVED}";
                    invokeApiOrderHeaderPut = new InvokeApi<OrderHeader>(configuration);
                    apiVmOrderHeaderPut = await invokeApiOrderHeaderPut.Invoke(requestUrl, HttpMethod.Put, orderHeader);
                }
            }
            SendMail(orderHeader);
            requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{orderHeader.UserId}";

            InvokeApi<IEnumerable<ShoppingCart>> invokeApiShoppingCarts = new InvokeApi<IEnumerable<ShoppingCart>>(configuration);
            ApiVM<IEnumerable<ShoppingCart>> apiVmShoppingCarts = await invokeApiShoppingCarts.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<ShoppingCart> shoppingCarts = apiVmShoppingCarts.TObject;
            requestUrl = $"api/ShoppingCart/DELETE/RemoveShoppingCarts";
            apiVmShoppingCarts = await invokeApiShoppingCarts.Invoke(requestUrl, HttpMethod.Delete, shoppingCarts);
            int cartCount = GetCartCount((ClaimsIdentity)User.Identity).Result;
            HttpContext.Session.SetInt32(GenericConstants.CartCountKey, cartCount);
            return View(orderHeaderId);
        }


        private double CalculateFinalPrice(int? cartItemCount, double? price, double? price50, double? price100)
        {
            if (cartItemCount <= 50)
            {
                return (double)price;
            }
            else if (cartItemCount > 50 && cartItemCount <= 100)
            {
                return (double)price50;
            }
            else
            {
                return (double)price100;
            }
        }

        private async Task<bool> SendMail(OrderHeader orderHeader)
        {
            string includeProperties = "Book,Book.Category,Book.CoverType";
            string requestUrl = $"api/ManageOrders/GET/GetOrderDetails/{orderHeader.OrderHeaderId}/{includeProperties}";
            InvokeApi<IEnumerable<OrderDetails>> invokeApiOrderDetails = new InvokeApi<IEnumerable<OrderDetails>>(configuration);
            ApiVM<IEnumerable<OrderDetails>> apiVmOrderDetails = await invokeApiOrderDetails.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<OrderDetails> orderDetails = apiVmOrderDetails.TObject;
            string emailTo = orderHeader.BooksForSaleUser.Email;
            string subject = "BooksForSale: Order placed successfully!";
            StringBuilder body = new StringBuilder();
            body.Append("<h1>Good day!</h1><br><h3>Your order is placed successfully with below books in it.</h3><br><br>");
            body.Append("<table border='1'><thead><tr><th>Order id</th><th>Book title</th><th>Price</th><th>Count</th></tr></thead>");
            body.Append("<tbody>");
            foreach(OrderDetails orderDetail in orderDetails)
            {
                body.Append("<tr>");
                body.Append($"<td>{orderHeader.OrderHeaderId}</td>");
                body.Append($"<td>{orderDetail.Book.Title}</td>");
                body.Append($"<td>{orderDetail.OrderPrice}</td>");
                body.Append($"<td>{orderDetail.Count}</td>");
                body.Append("</tr>");
            }
            body.Append("</tbody>");
            body.Append("</table><br><br>");
            body.Append("<h4>Address:</h4><br>");
            body.Append($"<p>{orderHeader.Name}</p>");
            body.Append($"<p>{orderHeader.PhoneNumber}</p>");
            body.Append($"<p>{orderHeader.StreetAddress}</p>");
            body.Append($"<p>{orderHeader.City}</p>");
            body.Append($"<p>{orderHeader.State}</p>");
            body.Append($"<p>{orderHeader.PostalCode}</p><br><br><br>");
            body.Append("<h6>Thank you for shopping with us!</h6><br><br>");
            body.Append("<p>Thanks and regards</p><br>");
            body.Append("<p>Team BooksForSale</p>");
            EmailSender emailSender = new EmailSender();
            emailSender.SendEmailAsync(emailTo, subject, body.ToString());
            return true;
        }

        [Authorize]
        public async Task<int> GetCartCount(ClaimsIdentity claimsIdentity)
        {
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;
            string requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{userId}";
            InvokeApi<IEnumerable<ShoppingCart>> invokeApi = new InvokeApi<IEnumerable<ShoppingCart>>(configuration);
            ApiVM<IEnumerable<ShoppingCart>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<ShoppingCart> shoppingCarts = apiVm.TObject;
            return shoppingCarts.Count();
        }
    }
}
