using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IBooksForSaleConfiguration booksForSaleConfiguration;
        public BookController(IBooksForSaleConfiguration booksForSaleConfiguration)
        {
            this.booksForSaleConfiguration = booksForSaleConfiguration;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpsertBook(int bookId = 0)
        {
            Book book = new Book();
            if(bookId != 0)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(booksForSaleConfiguration.BaseAddressForWebApi);
                string requestUrl = $"api/Book/GET/GetBook/{bookId}";
                book = await httpClient.GetFromJsonAsync<Book>(requestUrl);
            }
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertBook(Book book)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Book");
            }
            return View(book);
        }
    }
}
