using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Sainath.E_Commerce.BooksForSale.Web.HelperClasses;
using System.Diagnostics.CodeAnalysis;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN}")]
    public class CompanyController : Controller
    {
        private readonly IBooksForSaleConfiguration configuration;
        public CompanyController(IBooksForSaleConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpsertCompany(int? companyId)
        {
            Company company = new Company();
            if (companyId != 0 && companyId != null)
            {
                string requestUrl = $"api/Company/GET/Getcompany/{companyId}";
                InvokeApi<Company> invokeApi = new InvokeApi<Company>(configuration);
                ApiVM<Company> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
                company = apiVm.TObject;
                if (company != null)
                {
                    return View(company);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return View(company);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> UpsertCompany(Company company)
        {
            if (ModelState.IsValid)
            {
                InvokeApi<Company> invokeApi = new InvokeApi<Company>(configuration);
                if (company.CompanyId != 0)
                {
                    company.UpdatedDateTime = DateTime.Now;
                    string requestUrl = "api/Company/PUT/UpdateCompany";
                    ApiVM<Company> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Put, company);
                    HttpResponseMessage response = apiVm.Response;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Company updated successfully!";
                        return RedirectToAction("Index", "Company");
                    }
                }
                else
                {
                    string requestUrl = "api/Company/POST/InsertCompany";
                    ApiVM<Company> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Post, company);
                    HttpResponseMessage response = apiVm.Response;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Company created successfully!";
                        return RedirectToAction("Index", "Company");
                    }
                }
            }
            return View(company);
        }

        #region API ENDPOINTS

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            string requestUrl = "api/Company/GET/GetAllCompanies";
            InvokeApi<IEnumerable<Company>> invokeApi = new InvokeApi<IEnumerable<Company>>(configuration);
            ApiVM<IEnumerable<Company>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<Company> companies = apiVm.TObject;
            if (!companies.IsNullOrEmpty())
            {
                return Json(new { data = companies });
            }
            return Json(new { data = new List<Company>() });
        }

        [HttpDelete]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> DeleteCompanyApiEndPoint(int? companyId)
        {
            if(companyId != null)
            {
                string requestUrl = $"api/Company/GET/GetCompany/{companyId}";
                InvokeApi<Company> invokeApi = new InvokeApi<Company>(configuration);
                ApiVM<Company> apiVmGet = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
                Company company = apiVmGet.TObject; 
                if(company != null)
                {
                    requestUrl = "api/Company/DELETE/RemoveCompany";
                    ApiVM<Company> apiVmPost = await invokeApi.Invoke(requestUrl, HttpMethod.Delete, company);
                    HttpResponseMessage response = apiVmPost.Response;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Company deleted successfully!" });
                    }
                }
            }
            return Json( new { success = false, message = "Something went wrong while deleting the company!" } );
        }

        #endregion
    }
}
