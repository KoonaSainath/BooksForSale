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
