using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Sainath.E_Commerce.BooksForSale.Web.HelperClasses;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN}")]
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
            string requestUrl = "api/CoverType/GET/GetAllCoverTypes";
            InvokeApi<IEnumerable<CoverType>> invokeApi = new InvokeApi<IEnumerable<CoverType>>(configuration);
            ApiVM<IEnumerable<CoverType>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<CoverType> coverTypes = apiVm.TObject;
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
                string requestUrl = "api/CoverType/POST/InsertCoverType";
                InvokeApi<CoverType> invokeApi = new InvokeApi<CoverType>(configuration);
                ApiVM<CoverType> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Post, coverType);
                HttpResponseMessage response = apiVm.Response;
                if(response.IsSuccessStatusCode)
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
                string requestUrl = $"api/CoverType/GET/GetCoverType/{coverTypeId}";
                InvokeApi<CoverType> invokeApi = new InvokeApi<CoverType>(configuration);
                ApiVM<CoverType> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
                CoverType coverType = apiVm.TObject;
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
                string requestUrl = "api/CoverType/PUT/UpdateCoverType";
                coverType.UpdatedDateTime = DateTime.Now;
                InvokeApi<CoverType> invokeApi = new InvokeApi<CoverType>(configuration);
                ApiVM<CoverType> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Put, coverType);
                HttpResponseMessage response = apiVm.Response;
                if(response.IsSuccessStatusCode)
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
                string requestUrl = $"api/CoverType/GET/GetCoverType/{coverTypeId}";
                InvokeApi<CoverType> invokeApi = new InvokeApi<CoverType>(configuration);
                ApiVM<CoverType> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
                CoverType coverType = apiVm.TObject;
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
                string requestUrl = "api/CoverType/DELETE/RemoveCoverType";
                InvokeApi<CoverType> invokeApi = new InvokeApi<CoverType>(configuration);
                ApiVM<CoverType> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Delete, coverType);
                HttpResponseMessage response = apiVm.Response;
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
            string requestUrl = "api/CoverType/GET/GetAllCoverTypes";
            InvokeApi<IEnumerable<CoverType>> invokeApi = new InvokeApi<IEnumerable<CoverType>>(configuration);
            ApiVM<IEnumerable<CoverType>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<CoverType> coverTypes = apiVm.TObject;
            return Json(new { data = coverTypes });
        }
        [HttpDelete]
        public async Task<ActionResult> RemoveCoverTypeApiEndPoint(int coverTypeId)
        {
            string requestUrl = $"api/CoverType/GET/GetCoverType/{coverTypeId}";
            InvokeApi<CoverType> invokeApiGet = new InvokeApi<CoverType>(configuration);
            ApiVM<CoverType> apiVmGet = await invokeApiGet.Invoke(requestUrl, HttpMethod.Get);
            CoverType coverType = apiVmGet.TObject;
            requestUrl = "api/CoverType/DELETE/RemoveCoverType";
            InvokeApi<CoverType> invokeApiDelete = new InvokeApi<CoverType>(configuration);
            ApiVM<CoverType> apiVmDelete = await invokeApiGet.Invoke(requestUrl, HttpMethod.Delete, coverType);
            HttpResponseMessage response = apiVmDelete.Response;
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
