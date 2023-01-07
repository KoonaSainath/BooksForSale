using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
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
    }
}
