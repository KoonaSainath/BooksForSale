using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = $"api/Company/GET/Getcompany/{companyId}";
                company = await httpClient.GetFromJsonAsync<Company>(requestUrl);
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
        public async Task<IActionResult> UpsertCompany(Company company)
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);

                if (company.CompanyId != 0)
                {
                    company.UpdatedDateTime = DateTime.Now;
                    string requestUrl = "api/Company/PUT/UpdateCompany";
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync<Company>(requestUrl, company);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[Utility.Constants.GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Company updated successfully!";
                        return RedirectToAction("Index", "Company");
                    }
                }
                else
                {
                    string requestUrl = "api/Company/POST/InsertCompany";
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync<Company>(requestUrl, company);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData[Utility.Constants.GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Company created successfully!";
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
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = "api/Company/GET/GetAllCompanies";
            IEnumerable<Company> companies = await httpClient.GetFromJsonAsync<IEnumerable<Company>>(requestUrl);
            if (!companies.IsNullOrEmpty())
            {
                return Json(new { data = companies });
            }
            return Json(new { data = new List<Company>() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCompanyApiEndPoint(int? companyId)
        {
            if(companyId != null)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);

                string requestUrl = $"api/Company/GET/GetCompany/{companyId}";
                Company company = await httpClient.GetFromJsonAsync<Company>(requestUrl);
                if(company != null)
                {
                    requestUrl = "api/Company/DELETE/RemoveCompany";
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync<Company>(requestUrl, company);
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
