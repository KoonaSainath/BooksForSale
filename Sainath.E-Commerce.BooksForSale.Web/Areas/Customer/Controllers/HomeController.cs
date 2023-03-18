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
using Sainath.E_Commerce.BooksForSale.Web.HelperClasses;
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
            string includeProperties = "Category,CoverType";
            string requestUrl = $"api/Book/GET/GetAllBooks/{includeProperties}";
            InvokeApi<IEnumerable<Book>> invokeApi = new InvokeApi<IEnumerable<Book>>(configuration);
            ApiVM<IEnumerable<Book>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<Book> books = apiVm.TObject;
            return View(books);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int bookId)
        {
            if(bookId != 0)
            {
                string requestUrl = $"api/Book/GET/GetBook/{bookId}";
                InvokeApi<Book> invokeApiBook = new InvokeApi<Book>(configuration);
                ApiVM<Book> apiVmBook = await invokeApiBook.Invoke(requestUrl, HttpMethod.Get);
                Book book = apiVmBook.TObject;
                ClaimsIdentity claimsIdentity = (ClaimsIdentity) User.Identity;
                Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                string userId = claim.Value;
                string includeProperties = "Book,BooksForSaleUser,Book.Category,Book.CoverType";
                requestUrl = $"api/ShoppingCart/GET/GetShoppingCart/{bookId}/{userId}/0/{includeProperties}";
                InvokeApi<ShoppingCart> invokeApiShoppingCart = new InvokeApi<ShoppingCart>(configuration);
                ApiVM<ShoppingCart> apiVm = await invokeApiShoppingCart.Invoke(requestUrl, HttpMethod.Get);
                ShoppingCart shoppingCart = apiVm.TObject;
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
                ShoppingCartController shoppingCartController = new ShoppingCartController(configuration);
                InvokeApi<ShoppingCart> invokeApi = new InvokeApi<ShoppingCart>(configuration);
                if (cart.ShoppingCartId == 0)
                {
                    string requestUrl = "api/ShoppingCart/POST/InsertShoppingCart";
                    ApiVM<ShoppingCart> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Post, cart);
                    HttpResponseMessage response = apiVm.Response;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Shopping cart is inserted successfully!";
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
                    ApiVM<ShoppingCart> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Put, cart);
                    HttpResponseMessage response = apiVm.Response;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Shopping cart is updated successfully";
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