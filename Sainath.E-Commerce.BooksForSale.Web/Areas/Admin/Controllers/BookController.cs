using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels.Admin;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Sainath.E_Commerce.BooksForSale.Web.HelperClasses;
using System.Diagnostics.CodeAnalysis;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN}")]
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
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> UpsertBook(int bookId = 0)
        {
            BookVM bookVm = new BookVM();
            bookVm.Book = new Book();
            string requestUrl = $"api/Category/GET/GetAllCategories";
            InvokeApi<IEnumerable<Category>> invokeApiCategory = new InvokeApi<IEnumerable<Category>>(booksForSaleConfiguration);
            ApiVM<IEnumerable<Category>> apiVmCategory = await invokeApiCategory.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<Category> categories = apiVmCategory.TObject;
            IEnumerable<SelectListItem> categoriesSelectList = from c in categories
                                                               select new SelectListItem
                                                               {
                                                                   Text = c.CategoryName,
                                                                   Value = c.CategoryId.ToString()
                                                               };
            requestUrl = $"api/CoverType/GET/GetAllCoverTypes";
            InvokeApi<IEnumerable<CoverType>> invokeApiCoverType = new InvokeApi<IEnumerable<CoverType>>(booksForSaleConfiguration);
            ApiVM<IEnumerable<CoverType>> apiVmCoverType = await invokeApiCoverType.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<CoverType> coverTypes = apiVmCoverType.TObject;
            IEnumerable<SelectListItem> coverTypesSelectList = coverTypes.Select(c => new SelectListItem()
            {
                Text = c.CoverTypeName,
                Value = c.CoverTypeId.ToString()
            }) ;
            bookVm.Categories = categoriesSelectList;
            bookVm.CoverTypes = coverTypesSelectList;
            if (bookId != 0)
            {
                requestUrl = $"api/Book/GET/GetBook/{bookId}";
                InvokeApi<Book> invokeApiBook = new InvokeApi<Book>(booksForSaleConfiguration);
                ApiVM<Book> apiVmBook = await invokeApiBook.Invoke(requestUrl, HttpMethod.Get);
                Book book = apiVmBook.TObject;
                bookVm.Book = book;
            }
            return View(bookVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ExcludeFromCodeCoverage]
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
                string requestUrl = "";
                InvokeApi<Book> invokeApi = new InvokeApi<Book>(booksForSaleConfiguration);
               
                if (book.BookId != 0)
                {
                    book.UpdatedDateTime = DateTime.Now;
                    requestUrl = "api/Book/PUT/UpdateBook";
                    ApiVM<Book> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Put, book);
                    HttpResponseMessage response = apiVm.Response;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Book updated successfully";
                        return RedirectToAction("Index", "Book");
                    }
                }
                else
                {
                    requestUrl = "api/Book/POST/InsertBook";
                    ApiVM<Book> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Post, book);
                    HttpResponseMessage response = apiVm.Response;
                    if (response.IsSuccessStatusCode)
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
            string includeProperties = "Category,CoverType";
            string requestUrl = $"api/Book/GET/GetAllBooks/{includeProperties}";
            InvokeApi<List<Book>> invokeApi = new InvokeApi<List<Book>>(booksForSaleConfiguration);
            ApiVM<List<Book>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            List<Book> books = apiVm.TObject;
            return Json(new { data = books });
        }

        [HttpDelete]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> RemoveBookApiEndPoint(int bookId)
        {
            string requestUrl = $"api/Book/GET/GetBook/{bookId}";
            InvokeApi<Book> invokeApi = new InvokeApi<Book>(booksForSaleConfiguration);
            ApiVM<Book> apiVmGet = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            Book book = apiVmGet.TObject;
            if(book != null)
            {
                requestUrl = "api/Book/DELETE/RemoveBook";
                ApiVM<Book> apiVmDelete = await invokeApi.Invoke(requestUrl, HttpMethod.Delete, book);
                HttpResponseMessage response = apiVmDelete.Response;
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
