using Microsoft.AspNetCore.Mvc;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ManageOrders : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
