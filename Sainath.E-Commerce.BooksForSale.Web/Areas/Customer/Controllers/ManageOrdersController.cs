using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ManageOrdersController : Controller
    {
        private readonly IBooksForSaleConfiguration configuration;
        public ManageOrdersController(IBooksForSaleConfiguration configuration)
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
        public async Task<IActionResult> GetAllOrders()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;

            bool isUserAdminOrEmployee = false;
            if(User.IsInRole(GenericConstants.ROLE_ADMIN) || User.IsInRole(GenericConstants.ROLE_EMPLOYEE))
            {
                isUserAdminOrEmployee = true;
            }
            string includeProperties = "BooksForSaleUser";
            string requestUrl = $"api/ManageOrders/GET/GetAllOrders/{userId}/{isUserAdminOrEmployee}/{includeProperties}";
            IEnumerable<OrderHeader> orders = await httpClient.GetFromJsonAsync<IEnumerable<OrderHeader>>(requestUrl);

            return Json(new { data = orders });
        }

        #endregion
    }
}
