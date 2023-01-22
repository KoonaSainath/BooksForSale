using Microsoft.AspNetCore.Mvc;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
