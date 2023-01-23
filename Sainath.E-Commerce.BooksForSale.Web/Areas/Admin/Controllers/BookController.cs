using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UpsertBook()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpsertBook(Book book)
        {
            return View();
        }
    }
}
