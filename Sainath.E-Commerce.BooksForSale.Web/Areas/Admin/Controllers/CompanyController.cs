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
