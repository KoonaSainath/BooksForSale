using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Areas.Customer.Controllers;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Sainath.E_Commerce.BooksForSale.Web.Customer
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBooksForSaleConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IBooksForSaleConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string includeProperties = "Category,CoverType";
            string requestUrl = $"api/Book/GET/GetAllBooks/{includeProperties}";
            IEnumerable<Book> books = await httpClient.GetFromJsonAsync<IEnumerable<Book>>(requestUrl);
            return View(books);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int bookId)
        {
            if(bookId != 0)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = $"api/Book/GET/GetBook/{bookId}";
                Book book = await httpClient.GetFromJsonAsync<Book>(requestUrl);

                ClaimsIdentity claimsIdentity = (ClaimsIdentity) User.Identity;
                Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                string userId = claim.Value;

                ShoppingCart shoppingCart = null;
                string includeProperties = "Book,BooksForSaleUser,Book.Category,Book.CoverType";
                requestUrl = $"api/ShoppingCart/GET/GetShoppingCart/{bookId}/{userId}/0/{includeProperties}";
                shoppingCart = await httpClient.GetFromJsonAsync<ShoppingCart>(requestUrl);

                if(shoppingCart == null || (shoppingCart != null && shoppingCart.ShoppingCartId == 0))
                {
                    shoppingCart = new ShoppingCart();
                    shoppingCart.Book = book;
                    shoppingCart.BookId = bookId;
                    shoppingCart.Id = userId;
                } 
                return View(shoppingCart);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart cart)
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                ShoppingCartController shoppingCartController = new ShoppingCartController(configuration);

                if (cart.ShoppingCartId == 0)
                {
                    string requestUrl = "api/ShoppingCart/POST/InsertShoppingCart";
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync<ShoppingCart>(requestUrl, cart);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[Utility.Constants.GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Shopping cart is inserted successfully!";
                        int cartCount = shoppingCartController.GetCartCount((ClaimsIdentity)User.Identity).Result;
                        HttpContext.Session.SetInt32(GenericConstants.CartCountKey, cartCount);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    string requestUrl = "api/ShoppingCart/PUT/UpdateShoppingCart";
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync<ShoppingCart>(requestUrl, cart);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[Utility.Constants.GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Shopping cart is updated successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            return View(cart);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}