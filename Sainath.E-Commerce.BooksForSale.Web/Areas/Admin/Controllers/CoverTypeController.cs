using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IBooksForSaleConfiguration configuration;
        public CoverTypeController(IBooksForSaleConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = "api/CoverType/GET/GetAllCoverTypes";
            IEnumerable<CoverType> coverTypes = await httpClient.GetFromJsonAsync<IEnumerable<CoverType>>(requestUrl);
            return View(coverTypes);
        }
        [HttpGet]
        public async Task<IActionResult> InsertCoverType()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertCoverType(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = "api/CoverType/POST/InsertCoverType";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<CoverType>(requestUrl, coverType);
                Task<CoverType> insertedCoverTypeTask = response.Content.ReadFromJsonAsync<CoverType>();
                CoverType insertedCoverType = insertedCoverTypeTask.Result;
                if(insertedCoverType != null && response.IsSuccessStatusCode)
                {
                    ShowNotification("Cover type is created successfully!");
                    return RedirectToAction("Index", "CoverType");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCoverType(int? coverTypeId)
        {
            if(coverTypeId != 0 && coverTypeId != null)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = $"api/CoverType/GET/GetCoverType/{coverTypeId}";
                CoverType coverType = await httpClient.GetFromJsonAsync<CoverType>(requestUrl);
                return View(coverType);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCoverType(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = "api/CoverType/PUT/UpdateCoverType";
                coverType.UpdatedDateTime = DateTime.Now;
                HttpResponseMessage response = await httpClient.PutAsJsonAsync<CoverType>(requestUrl, coverType);
                CoverType updatedCoverType = response.Content.ReadFromJsonAsync<CoverType>().Result;
                if(response.IsSuccessStatusCode && updatedCoverType != null)
                {
                    ShowNotification("Cover type is updated successfully!");
                    return RedirectToAction("Index", "CoverType");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RemoveCoverType(int? coverTypeId)
        {
            if(coverTypeId != 0 && coverTypeId != null)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = $"api/CoverType/GET/GetCoverType/{coverTypeId}";
                CoverType coverType = await httpClient.GetFromJsonAsync<CoverType>(requestUrl);
                return View(coverType);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCoverType(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = "api/CoverType/DELETE/RemoveCoverType";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<CoverType>(requestUrl, coverType);
                if (response.IsSuccessStatusCode)
                {
                    ShowNotification("Cover type is deleted successfully!");
                    return RedirectToAction("Index", "CoverType");
                }
            }
            return View();
        }
        private void ShowNotification(string message)
        {
            TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = message;
        }

        #region API END POINTS
        [HttpGet]
        public async Task<ActionResult> GetAllCoverTypesApiEndPoint()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = "api/CoverType/GET/GetAllCoverTypes";
            IEnumerable<CoverType> coverTypes = await httpClient.GetFromJsonAsync<IEnumerable<CoverType>>(requestUrl);
            return Json(new { data = coverTypes });
        }
        [HttpDelete]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveCoverTypeApiEndPoint(int coverTypeId)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = $"api/CoverType/GET/GetCoverType/{coverTypeId}";
            CoverType coverType = await httpClient.GetFromJsonAsync<CoverType>(requestUrl);

            requestUrl = "api/CoverType/DELETE/RemoveCoverType";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync<CoverType>(requestUrl, coverType);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Cover type removed successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Error occured while removing the cover type!" });
            }
        }

        #endregion
    }
}
