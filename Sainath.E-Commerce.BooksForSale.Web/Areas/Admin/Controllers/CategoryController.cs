using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Category> categories = new List<Category>()
            {
                new Category { CategoryName = "Action", DisplayOrder = 1, CreatedDateTime = DateTime.Now, UpdatedDateTime = DateTime.Now }
            };
            return View(categories);
        }
    }
}
