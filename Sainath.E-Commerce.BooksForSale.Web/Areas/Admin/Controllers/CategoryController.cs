using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public async Task<IActionResult> Index()
        {
            string requestUrl = "api/Category/GET/GetAllCategories";
            string baseAddress = "https://localhost:7138";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(baseAddress);
            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
            Task<IEnumerable<Category>> categoriesTask = response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
            IEnumerable<Category> categories = categoriesTask.Result;
            return View(categories);
        }
    }
}
