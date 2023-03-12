using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Utility.Extensions;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Stripe;
using Stripe.Checkout;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

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
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE}")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE}")]
        public async Task<IActionResult> StartProcessingOrder()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = $"api/ManageOrders/PUT/StartProcessingOrder";
            HttpResponseMessage response = await httpClient.PutAsJsonAsync<OrderHeader>(requestUrl, OrderVM.OrderHeader);
            string responseMessage = response.Content.ReadAsStringAsync().Result.NullCheckTrim();
            responseMessage = responseMessage.TrimStart('"').TrimEnd('"');
            TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = responseMessage;
            return RedirectToAction(nameof(GetOrder), new { orderHeaderId = OrderVM.OrderHeader.OrderHeaderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE}")]
        public async Task<IActionResult> ShipOrder()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            OrderVM.OrderHeader.OrderStatus = OrderStatus.STATUS_SHIPPED;
            OrderVM.OrderHeader.ShippingDate = DateTime.Now;
            if (OrderVM.OrderHeader.BooksForSaleUser.CompanyId.GetValueOrDefault() != 0)
            {
                OrderVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            string requestUrl = $"api/ManageOrders/PUT/ShipOrder";
            HttpResponseMessage response = await httpClient.PutAsJsonAsync<OrderHeader>(requestUrl, OrderVM.OrderHeader);
            string responseMessage = response.Content.ReadAsStringAsync().Result.NullCheckTrim();
            TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = responseMessage;
            return RedirectToAction(nameof(GetOrder), new { orderHeaderId = OrderVM.OrderHeader.OrderHeaderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE}")]
        public async Task<IActionResult> MakePayment()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string includeProperties = "Book,Book.Category,Book.CoverType";
            string requestUrl = $"api/ManageOrders/GET/GetOrderDetails/{OrderVM.OrderHeader.OrderHeaderId}/{includeProperties}";
            OrderVM.ListOfOrderDetails = await httpClient.GetFromJsonAsync<IEnumerable<OrderDetails>>(requestUrl);

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = configuration.BaseAddressForWebApplication + $"Customer/ManageOrders/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.OrderHeaderId}",
                CancelUrl = configuration.BaseAddressForWebApplication + $"Customer/ManageOrders/GetOrder?orderHeaderId={OrderVM.OrderHeader.OrderHeaderId}",
            };

            foreach (OrderDetails orderDetails in OrderVM.ListOfOrderDetails)
            {
                options.LineItems.Add(new SessionLineItemOptions()
                {
                    Quantity = orderDetails.Count,
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        Currency = "inr",
                        UnitAmount = (long)(orderDetails.OrderPrice * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = orderDetails.Book.Title
                        }
                    },
                });
            }

            SessionService service = new SessionService();
            Session session = service.Create(options);

            session.PaymentIntentId = "Will be updated on order confirmation";
            requestUrl = $"api/ShoppingCart/PUT/UpdateStripeStatus/{OrderVM.OrderHeader.OrderHeaderId}/{session.Id}/{session.PaymentIntentId}";
            HttpResponseMessage response = await httpClient.PutAsJsonAsync<OrderHeader>(requestUrl, OrderVM.OrderHeader);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpGet]
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE}")]
        public async Task<IActionResult> PaymentConfirmation(int orderHeaderId)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
            string requestUrl = $"api/ManageOrders/GET/GetOrder/{orderHeaderId}";
            OrderHeader orderHeader = await httpClient.GetFromJsonAsync<OrderHeader>(requestUrl);
            if (orderHeader != null)
            {
                SessionService sessionService = new SessionService();
                Session session = sessionService.Get(orderHeader.StripeSessionId);

                if (session.PaymentStatus.NullCheckTrim().ToLower() == OrderStatus.PAYMENT_STATUS_PAID.ToLower())
                {
                    requestUrl = $"api/ShoppingCart/PUT/UpdateStripeStatus/{orderHeaderId}/{orderHeader.StripeSessionId}/{session.PaymentIntentId}";
                    await httpClient.PutAsJsonAsync<OrderHeader>(requestUrl, orderHeader);

                    requestUrl = $"api/ShoppingCart/PUT/UpdateOrderHeaderStatus/{orderHeaderId}/{orderHeader.OrderStatus}/{OrderStatus.PAYMENT_STATUS_APPROVED}";
                    await httpClient.PutAsJsonAsync<OrderHeader>(requestUrl, orderHeader);
                }
            }
            return View(orderHeaderId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE}")]
        public async Task<IActionResult> CancelOrder()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);

            string requestUrl = string.Empty;
            HttpResponseMessage response = new HttpResponseMessage();
            bool isRefundProcessed = false;

            if (OrderVM.OrderHeader.PaymentStatus.NullCheckTrim().ToLower() == OrderStatus.PAYMENT_STATUS_APPROVED.ToLower())
            {
                var options = new RefundCreateOptions()
                {
                    PaymentIntent = OrderVM.OrderHeader.StripePaymentIntentId,
                    Reason = RefundReasons.RequestedByCustomer
                };
                RefundService refundService = new RefundService();
                Refund refund = refundService.Create(options);

                isRefundProcessed = true;
                requestUrl = $"api/ManageOrders/PUT/CancelOrder/{isRefundProcessed}";

            }
            else
            {
                requestUrl = $"api/ManageOrders/PUT/CancelOrder/{isRefundProcessed}";
            }
            response = await httpClient.PutAsJsonAsync<OrderHeader>(requestUrl, OrderVM.OrderHeader);
            if (response.IsSuccessStatusCode)
            {
                TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = response.Content.ReadAsStringAsync().Result.NullCheckTrim().TrimStart('"').TrimEnd('"');
            }
            else
            {
                TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = "Failed to cancel the order. Please try again after sometime.";
            }
            return RedirectToAction(nameof(GetOrder), new { OrderVM.OrderHeader.OrderHeaderId });
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
            if (User.IsInRole(GenericConstants.ROLE_ADMIN) || User.IsInRole(GenericConstants.ROLE_EMPLOYEE))
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
