using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN}")]
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
                    ShowNotification("Category is created successfully!");
                    return RedirectToAction("Index", "Category");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int? categoryId)
        {
            if (categoryId != 0 && categoryId != null)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = $"api/Category/GET/GetCategoryById/{categoryId}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    Task<Category> categoryTask = response.Content.ReadFromJsonAsync<Category>();
                    Category category = categoryTask.Result;
                    if (category != null)
                    {
                        return View(category);
                    }
                }
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = "api/Category/PUT/UpdateCategory";
                category.UpdatedDateTime = DateTime.Now;
                HttpResponseMessage response = await httpClient.PutAsJsonAsync<Category>(requestUrl, category);
                if (response.IsSuccessStatusCode)
                {
                    ShowNotification("Category is updated successfully!");
                    return RedirectToAction("Index", "Category");
                }
            }
            return View();
        }
        private void ShowNotification(string message)
        {
            TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = message;
        }

        #region API ENDPOINTS

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesApiEndPoint()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = "api/Category/GET/GetAllCategories";
            IEnumerable<Category> categories = await httpClient.GetFromJsonAsync<IEnumerable<Category>>(requestUrl);
            return Json(new { data = categories });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCategoryApiEndPoint(int categoryId)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = $"api/Category/GET/GetCategoryById/{categoryId}";
            Category category = await httpClient.GetFromJsonAsync<Category>(requestUrl);
            if (category != null)
            {
                requestUrl = "api/Category/DELETE/RemoveCategory";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<Category>(requestUrl, category);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Category removed successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Error occured while removing the category!" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Error occured while removing the category!" });
            }
        }

        #endregion
    }
}
