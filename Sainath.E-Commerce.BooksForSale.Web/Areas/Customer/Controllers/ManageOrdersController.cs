using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Utility.Extensions;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Sainath.E_Commerce.BooksForSale.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ManageOrdersController : Controller
    {
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        private readonly IBooksForSaleConfiguration configuration;
        public ManageOrdersController(IBooksForSaleConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index(string? status = null)
        {
            return View(nameof(Index), status);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder(int orderHeaderId)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);

            string includeProperties = "BooksForSaleUser";
            string requestUrl = $"api/ManageOrders/GET/GetOrder/{orderHeaderId}/{includeProperties}";
            OrderHeader orderHeader = await httpClient.GetFromJsonAsync<OrderHeader>(requestUrl);

            includeProperties = "Book,Book.Category,Book.CoverType";
            requestUrl = $"api/ManageOrders/GET/GetOrderDetails/{orderHeaderId}/{includeProperties}";
            IEnumerable<OrderDetails> orderDetails = await httpClient.GetFromJsonAsync<IEnumerable<OrderDetails>>(requestUrl);

            OrderVM = new OrderVM()
            {
                OrderHeader = orderHeader,
                ListOfOrderDetails = orderDetails
            };
            return View(OrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderDetails()
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string requestUrl = $"api/ManageOrders/GET/GetOrder/{OrderVM.OrderHeader.OrderHeaderId}";
                OrderHeader orderHeaderFromDb = await httpClient.GetFromJsonAsync<OrderHeader>(requestUrl);

                orderHeaderFromDb.Name = OrderVM.OrderHeader.Name.NullCheckTrim();
                orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber.NullCheckTrim();
                orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress.NullCheckTrim();
                orderHeaderFromDb.City = OrderVM.OrderHeader.City.NullCheckTrim();
                orderHeaderFromDb.State = OrderVM.OrderHeader.State.NullCheckTrim();
                orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode.NullCheckTrim();

                if (OrderVM.OrderHeader.Carrier != null)
                {
                    orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier.NullCheckTrim();
                }
                if (OrderVM.OrderHeader.TrackingNumber != null)
                {
                    orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber.NullCheckTrim();
                }

                requestUrl = $"api/ManageOrders/PUT/UpdateOrder";
                HttpResponseMessage response = await httpClient.PutAsJsonAsync<OrderHeader>(requestUrl, orderHeaderFromDb);
                if (response.IsSuccessStatusCode)
                {
                    TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Order details are updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Failed to update order details!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Failed to update as you didn't enter information for one or more mandatory fields";
                return RedirectToAction(nameof(GetOrder), new { orderHeaderId = OrderVM.OrderHeader.OrderHeaderId });
            }
        }

        #region API ENDPOINTS

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(string status)
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
            string requestUrl = $"api/ManageOrders/GET/GetAllOrders/{userId}/{isUserAdminOrEmployee}/{status}/{includeProperties}";
            IEnumerable<OrderHeader> orders = await httpClient.GetFromJsonAsync<IEnumerable<OrderHeader>>(requestUrl);

            return Json(new { data = orders });
        }

        #endregion
    }
}
