using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            BookVM bookVm = new BookVM();
            bookVm.Book = new Book();

            // fill bookVm.Categories with list of all categories 

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(booksForSaleConfiguration.BaseAddressForWebApi);
            string requestUrl = $"api/Category/GET/GetAllCategories";
            IEnumerable<Category> categories = await httpClient.GetFromJsonAsync<IEnumerable<Category>>(requestUrl);
            IEnumerable<SelectListItem> categoriesSelectList = from c in categories
                                                               select new SelectListItem
                                                               {
                                                                   Text = c.CategoryName,
                                                                   Value = c.CategoryId.ToString()
                                                               };

            // fill bookVm.CoverTypes with list of all covertypes
            requestUrl = $"api/CoverType/GET/GetAllCoverTypes";
            IEnumerable<CoverType> coverTypes = await httpClient.GetFromJsonAsync<IEnumerable<CoverType>>(requestUrl);
            IEnumerable<SelectListItem> coverTypesSelectList = coverTypes.Select(c => new SelectListItem()
            {
                Text = c.CoverTypeName,
                Value = c.CoverTypeId.ToString()
            }) ;

            bookVm.Categories = categoriesSelectList;
            bookVm.CoverTypes = coverTypesSelectList;

            //In case of update, bookId will be an existing book's id
            if (bookId != 0)
            {
                requestUrl = $"api/Book/GET/GetBook/{bookId}";
                Book book = await httpClient.GetFromJsonAsync<Book>(requestUrl);
                bookVm.Book = book;
            }
            return View(bookVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertBook(BookVM bookVm, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Book");
            }
            return View(bookVm);
        }
    }
}
