using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sainath.E_Commerce.BooksForSale.Models.Models;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                if (company.CompanyId != 0)
                {

                }
                else
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
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

        #endregion
    }
}
