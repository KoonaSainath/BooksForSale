using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IBooksForSaleConfiguration booksForSaleConfiguration;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BookController(IBooksForSaleConfiguration booksForSaleConfiguration, IWebHostEnvironment webHostEnvironment)
        {
            this.booksForSaleConfiguration = booksForSaleConfiguration;
            this.webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> UpsertBook(BookVM bookVm, IFormFile imageFile)
        {
            ModelState.Remove("imageFile");
            if (ModelState.IsValid)
            {
                Book book = bookVm.Book;

                if(imageFile != null)
                {
                    string fileName = imageFile.FileName;
                    string imageExtension = Path.GetExtension(fileName);
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string storagePath = @"images\ImagesOfBooks";
                    string uniqueFileName = Guid.NewGuid().ToString() + imageExtension;
                    string imageUrlForDb = Path.Combine(storagePath, uniqueFileName);
                    string finalPath = Path.Combine(wwwRootPath, imageUrlForDb);
                    using (FileStream fileStream = new FileStream(finalPath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    //already an image exists. so its an update
                    if(book.ImageUrl != null)
                    {
                        string pathToDelete = Path.Combine(wwwRootPath, book.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(pathToDelete))
                        {
                            System.IO.File.Delete(pathToDelete);
                        }
                    }

                    book.ImageUrl = $"\\images\\ImagesOfBooks\\{uniqueFileName}";
                    
                }
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(booksForSaleConfiguration.BaseAddressForWebApi);
                string requestUrl = "";
                if (book.BookId != 0)
                {
                    book.UpdatedDateTime = DateTime.Now;
                    requestUrl = "api/Book/PUT/UpdateBook";
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync<Book>(requestUrl, book);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Book updated successfully";
                        return RedirectToAction("Index", "Book");
                    }
                }
                else
                {
                    requestUrl = "api/Book/POST/InsertBook";
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync<Book>(requestUrl, book); if (response.IsSuccessStatusCode)
                    {
                        TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Book created successfully";
                        return RedirectToAction("Index", "Book");
                    }
                }
            }
            return View(bookVm);
        }

        #region API ENDPOINTS
        [HttpGet]
        public async Task<IActionResult> GetAllBooksApiEndPoint()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(booksForSaleConfiguration.BaseAddressForWebApi);
            string includeProperties = "Category,CoverType";
            string requestUrl = $"api/Book/GET/GetAllBooks/{includeProperties}";
            List<Book> books = await httpClient.GetFromJsonAsync<List<Book>>(requestUrl);
            return Json(new { data = books });
        }

        
        [HttpDelete]
        public async Task<IActionResult> RemoveBookApiEndPoint(int bookId)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(booksForSaleConfiguration.BaseAddressForWebApi);

            string requestUrl = $"api/Book/GET/GetBook/{bookId}";
            Book book = await httpClient.GetFromJsonAsync<Book>(requestUrl);

            if(book != null)
            {
                requestUrl = "api/Book/DELETE/RemoveBook";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<Book>(requestUrl, book);
                if (response.IsSuccessStatusCode)
                {
                    string bookImagePathToDelete = Path.Combine(webHostEnvironment.WebRootPath, book.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(bookImagePathToDelete))
                    {
                        System.IO.File.Delete(bookImagePathToDelete);
                    }
                    return Json(new { success = true, message = "Book removed successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Error occured while removing the book!" });
                }
            }
            return Json(new { success = false, message = "Error occured while removing the book!" });
        }

        #endregion
    }
}
