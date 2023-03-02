using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Stripe.Checkout;
using System.Net.Http.Headers;
using System.Security.Claims;

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

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);

            string requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{userId}";
            IEnumerable<ShoppingCart> shoppingCarts = await httpClient.GetFromJsonAsync<IEnumerable<ShoppingCart>>(requestUrl);

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
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = $"api/ShoppingCart/GET/GetShoppingCart/0/0/{shoppingCartId}";
            ShoppingCart shoppingCart = await httpClient.GetFromJsonAsync<ShoppingCart>(requestUrl);

            if (shoppingCart != null && shoppingCart.CartItemCount < 200)
            {
                requestUrl = $"api/ShoppingCart/PUT/IncrementBookCountInShoppingCart";
                HttpResponseMessage response = await httpClient.PutAsJsonAsync<ShoppingCart>(requestUrl, shoppingCart);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DecrementBookCountInShoppingCart(int shoppingCartId)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = $"api/ShoppingCart/GET/GetshoppingCart/0/0/{shoppingCartId}";
            ShoppingCart shoppingCart = await httpClient.GetFromJsonAsync<ShoppingCart>(requestUrl);

            if (shoppingCart != null && shoppingCart.CartItemCount > 1)
            {
                requestUrl = "api/ShoppingCart/PUT/DecrementBookCountInShoppingCart";
                HttpResponseMessage response = await httpClient.PutAsJsonAsync<ShoppingCart>(requestUrl, shoppingCart);
            }
            else if (shoppingCart != null && shoppingCart.CartItemCount <= 1)
            {
                requestUrl = "api/ShoppingCart/DELETE/RemoveShoppingCart";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<ShoppingCart>(requestUrl, shoppingCart);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveBookFromShoppingCart(int shoppingCartId)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = $"api/ShoppingCart/GET/GetShoppingCart/0/0/{shoppingCartId}";
            ShoppingCart shoppingCart = await httpClient.GetFromJsonAsync<ShoppingCart>(requestUrl);
            if (shoppingCart != null)
            {
                requestUrl = "api/ShoppingCart/DELETE/RemoveShoppingCart";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<ShoppingCart>(requestUrl, shoppingCart);
            }
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

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);

            string requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{userId}";
            ShoppingCartVM.ShoppingCarts = await httpClient.GetFromJsonAsync<IEnumerable<ShoppingCart>>(requestUrl);

            ShoppingCartVM.OrderHeader.TotalOrderAmount = 0;
            foreach (ShoppingCart shoppingCart in ShoppingCartVM.ShoppingCarts)
            {
                shoppingCart.Price = CalculateFinalPrice(shoppingCart.CartItemCount, shoppingCart.Book.Price, shoppingCart.Book.Price50, shoppingCart.Book.Price100);
                ShoppingCartVM.OrderHeader.TotalOrderAmount += shoppingCart.Price;
            }

            requestUrl = $"api/BooksForSaleUser/GET/GetUser/{userId}";
            ShoppingCartVM.OrderHeader.BooksForSaleUser = await httpClient.GetFromJsonAsync<BooksForSaleUser>(requestUrl);

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

                ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
                ShoppingCartVM.OrderHeader.OrderStatus = OrderStatus.STATUS_PENDING;

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);

                string requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{ShoppingCartVM.OrderHeader.UserId}";
                ShoppingCartVM.ShoppingCarts = await httpClient.GetFromJsonAsync<IEnumerable<ShoppingCart>>(requestUrl);
                ShoppingCartVM.OrderHeader.TotalOrderAmount = 0;
                foreach (ShoppingCart cart in ShoppingCartVM.ShoppingCarts)
                {
                    cart.BooksForSaleUser = null;
                    cart.Price = CalculateFinalPrice(cart.CartItemCount, cart.Book.Price, cart.Book.Price50, cart.Book.Price100);
                    ShoppingCartVM.OrderHeader.TotalOrderAmount += (double)(cart.Price * cart.CartItemCount);
                }

                requestUrl = "api/ShoppingCart/POST/InsertOrderHeader";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<OrderHeader>(requestUrl, ShoppingCartVM.OrderHeader);
                if (response.IsSuccessStatusCode)
                {
                    OrderHeader orderHeader = response.Content.ReadFromJsonAsync<OrderHeader>().Result;
                    requestUrl = "api/ShoppingCart/POST/InsertOrderDetails";
                    foreach (ShoppingCart cart in ShoppingCartVM.ShoppingCarts)
                    {
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.OrderHeaderId = orderHeader.OrderHeaderId;
                        orderDetails.BookId = cart.BookId;
                        orderDetails.Count = (int)cart.CartItemCount;
                        orderDetails.OrderPrice = (double)(cart.Price * cart.CartItemCount);
                        response = await httpClient.PostAsJsonAsync<OrderDetails>(requestUrl, orderDetails);
                    }
                }

                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = configuration.BaseAddressForWebApplication + $"Customer/ShoppingCart/OrderConfirmation?orderHeaderId={ShoppingCartVM.OrderHeader.OrderHeaderId}",
                    CancelUrl = configuration.BaseAddressForWebApplication + "Customer/ShoppingCart/Index",
                };

                foreach(ShoppingCart cart in ShoppingCartVM.ShoppingCarts)
                {
                    options.LineItems.Add(new SessionLineItemOptions()
                    {
                        Quantity = cart.CartItemCount,
                        PriceData = new SessionLineItemPriceDataOptions()
                        {
                            Currency = "inr",
                            UnitAmount = (long)cart.Price,
                            ProductData = new SessionLineItemPriceDataProductDataOptions()
                            {
                                Name = cart.Book.Title
                            }
                        },
                    });
                }

                var service = new SessionService();
                Session session = service.Create(options);

                ShoppingCartVM.OrderHeader.StripeSessionId = session.Id;
                ShoppingCartVM.OrderHeader.StripePaymentIntentId = session.PaymentIntentId;

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            return View();
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
    }
}
