using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Sainath.E_Commerce.BooksForSale.Web.HelperClasses;

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
            InvokeApi<IEnumerable<Category>> invokeApi = new InvokeApi<IEnumerable<Category>>(configuration);
            ApiVM<IEnumerable<Category>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<Category> categories = apiVm.TObject;
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
                InvokeApi<Category> invokeApi = new InvokeApi<Category>(configuration);
                ApiVM<Category> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Post, category);
                HttpResponseMessage response = apiVm.Response;
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
                string requestUrl = $"api/Category/GET/GetCategoryById/{categoryId}";
                InvokeApi<Category> invokeApi = new InvokeApi<Category>(configuration);
                ApiVM<Category> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
                Category category = apiVm.TObject;
                return View(category);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                string requestUrl = "api/Category/PUT/UpdateCategory";
                category.UpdatedDateTime = DateTime.Now;
                InvokeApi<Category> invokeApi = new InvokeApi<Category>(configuration);
                ApiVM<Category> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Put, category);
                HttpResponseMessage response = apiVm.Response;
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
            string requestUrl = "api/Category/GET/GetAllCategories";
            InvokeApi<IEnumerable<Category>> invokeApi = new InvokeApi<IEnumerable<Category>>(configuration);
            ApiVM<IEnumerable<Category>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<Category> categories = apiVm.TObject;
            return Json(new { data = categories });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCategoryApiEndPoint(int categoryId)
        {
            string requestUrl = $"api/Category/GET/GetCategoryById/{categoryId}";
            InvokeApi<Category> invokeApi = new InvokeApi<Category>(configuration);
            ApiVM<Category> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            Category category = apiVm.TObject;
            if (category != null)
            {
                requestUrl = "api/Category/DELETE/RemoveCategory";
                InvokeApi<Category> invokeApiDelete = new InvokeApi<Category>(configuration);
                ApiVM<Category> apiVmDelete = await invokeApiDelete.Invoke(requestUrl, HttpMethod.Delete, category);
                HttpResponseMessage response = apiVmDelete.Response;
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
