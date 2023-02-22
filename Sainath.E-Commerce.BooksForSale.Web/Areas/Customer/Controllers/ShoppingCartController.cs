using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels.Customer;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IBooksForSaleConfiguration configuration;
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

            shoppingCartVM.TotalPrice = 0;
            foreach (ShoppingCart cart in shoppingCarts)
            {
                cart.Price = CalculateFinalPrice(cart.CartItemCount, cart.Book.Price, cart.Book.Price50, cart.Book.Price100);
                shoppingCartVM.TotalPrice += (double)(cart.Price * cart.CartItemCount);
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

            if(shoppingCart != null && shoppingCart.CartItemCount < 200)
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

            if(shoppingCart != null && shoppingCart.CartItemCount > 1)
            {
                requestUrl = "api/ShoppingCart/PUT/DecrementBookCountInShoppingCart";
                HttpResponseMessage response = await httpClient.PutAsJsonAsync<ShoppingCart>(requestUrl, shoppingCart);
            }
            else if(shoppingCart != null && shoppingCart.CartItemCount <= 1)
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
            if(shoppingCart != null)
            {
                requestUrl = "api/ShoppingCart/DELETE/RemoveShoppingCart";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<ShoppingCart>(requestUrl, shoppingCart);
            }
            return RedirectToAction(nameof(Index));
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
