using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IBooksForSaleConfiguration configuration;
        public CategoryController(IBooksForSaleConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            string requestUrl = "api/Category/GET/GetAllCategories";
            string baseAddress = configuration.BaseAddressForWebApi;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(baseAddress);
            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
            Task<IEnumerable<Category>> categoriesTask = response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
            IEnumerable<Category> categories = categoriesTask.Result;
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> InsertCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                string requestUrl = "api/Category/POST/InsertCategory";
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<Category>(requestUrl, category);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Category");
                }
            }
            return View();
        }
    }
}
