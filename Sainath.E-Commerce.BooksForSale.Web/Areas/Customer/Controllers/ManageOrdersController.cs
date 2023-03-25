using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Utility.Extensions;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Sainath.E_Commerce.BooksForSale.Web.HelperClasses;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics.CodeAnalysis;
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
            string includeProperties = "BooksForSaleUser";
            string requestUrl = $"api/ManageOrders/GET/GetOrder/{orderHeaderId}/{includeProperties}";
            InvokeApi<OrderHeader> invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
            ApiVM<OrderHeader> apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Get);
            OrderHeader orderHeader = apiVmOrderHeader.TObject;
            includeProperties = "Book,Book.Category,Book.CoverType";
            requestUrl = $"api/ManageOrders/GET/GetOrderDetails/{orderHeaderId}/{includeProperties}";

            InvokeApi<IEnumerable<OrderDetails>> invokeApiOrderDetails = new InvokeApi<IEnumerable<OrderDetails>>(configuration);
            ApiVM<IEnumerable<OrderDetails>> apiVmOrderDetails = await invokeApiOrderDetails.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<OrderDetails> orderDetails = apiVmOrderDetails.TObject;
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
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> UpdateOrderDetails()
        {
            if (ModelState.IsValid)
            {
                string requestUrl = $"api/ManageOrders/GET/GetOrder/{OrderVM.OrderHeader.OrderHeaderId}";

                InvokeApi<OrderHeader> invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
                ApiVM<OrderHeader> apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Get);
                OrderHeader orderHeaderFromDb = apiVmOrderHeader.TObject;
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
                apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Put, orderHeaderFromDb);
                HttpResponseMessage response = apiVmOrderHeader.Response;
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
            string requestUrl = $"api/ManageOrders/PUT/StartProcessingOrder";
            InvokeApi<OrderHeader> invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
            ApiVM<OrderHeader> apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Put, OrderVM.OrderHeader);
            HttpResponseMessage response = apiVmOrderHeader.Response;
            string responseMessage = response.Content.ReadAsStringAsync().Result.NullCheckTrim();
            responseMessage = responseMessage.TrimStart('"').TrimEnd('"');
            TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = responseMessage;
            return RedirectToAction(nameof(GetOrder), new { orderHeaderId = OrderVM.OrderHeader.OrderHeaderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE}")]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> ShipOrder()
        {
            OrderVM.OrderHeader.OrderStatus = OrderStatus.STATUS_SHIPPED;
            OrderVM.OrderHeader.ShippingDate = DateTime.Now;
            if (OrderVM.OrderHeader.BooksForSaleUser.CompanyId.GetValueOrDefault() != 0)
            {
                OrderVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            string requestUrl = $"api/ManageOrders/PUT/ShipOrder";
            InvokeApi<OrderHeader> invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
            ApiVM<OrderHeader> apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Put, OrderVM.OrderHeader);
            HttpResponseMessage response = apiVmOrderHeader.Response;
            string responseMessage = response.Content.ReadAsStringAsync().Result.NullCheckTrim();
            responseMessage = responseMessage.TrimStart('"').TrimEnd('"');
            TempData[GenericConstants.NOTIFICATION_MESSAGE_KEY] = responseMessage;
            return RedirectToAction(nameof(GetOrder), new { orderHeaderId = OrderVM.OrderHeader.OrderHeaderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE},{GenericConstants.ROLE_COMPANY_CUSTOMER}")]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> MakePayment()
        {
            string includeProperties = "Book,Book.Category,Book.CoverType";
            string requestUrl = $"api/ManageOrders/GET/GetOrderDetails/{OrderVM.OrderHeader.OrderHeaderId}/{includeProperties}";
            InvokeApi<IEnumerable<OrderDetails>> invokeApiOrderDetails = new InvokeApi<IEnumerable<OrderDetails>>(configuration);
            ApiVM<IEnumerable<OrderDetails>> apiVmOrderDetails = await invokeApiOrderDetails.Invoke(requestUrl, HttpMethod.Get);
            OrderVM.ListOfOrderDetails = apiVmOrderDetails.TObject;
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
            InvokeApi<OrderHeader> invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
            ApiVM<OrderHeader> apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Put, OrderVM.OrderHeader);
            HttpResponseMessage response = apiVmOrderHeader.Response;
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpGet]
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE},{GenericConstants.ROLE_COMPANY_CUSTOMER}")]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> PaymentConfirmation(int orderHeaderId)
        {
            string requestUrl = $"api/ManageOrders/GET/GetOrder/{orderHeaderId}";
            InvokeApi<OrderHeader> invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
            ApiVM<OrderHeader> apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Get);
            OrderHeader orderHeader = apiVmOrderHeader.TObject;
            if (orderHeader != null)
            {
                SessionService sessionService = new SessionService();
                Session session = sessionService.Get(orderHeader.StripeSessionId);
                if (session.PaymentStatus.NullCheckTrim().ToLower() == OrderStatus.PAYMENT_STATUS_PAID.ToLower())
                {
                    requestUrl = $"api/ShoppingCart/PUT/UpdateStripeStatus/{orderHeaderId}/{orderHeader.StripeSessionId}/{session.PaymentIntentId}";
                    invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
                    await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Put, orderHeader);
                    requestUrl = $"api/ShoppingCart/PUT/UpdateOrderHeaderStatus/{orderHeaderId}/{orderHeader.OrderStatus}/{OrderStatus.PAYMENT_STATUS_APPROVED}";
                    invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
                    await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Put, orderHeader);
                }
            }
            return View(orderHeaderId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{GenericConstants.ROLE_ADMIN},{GenericConstants.ROLE_EMPLOYEE}")]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> CancelOrder()
        {
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
            InvokeApi<OrderHeader> invokeApiOrderHeader = new InvokeApi<OrderHeader>(configuration);
            ApiVM<OrderHeader> apiVmOrderHeader = await invokeApiOrderHeader.Invoke(requestUrl, HttpMethod.Put, OrderVM.OrderHeader);
            response = apiVmOrderHeader.Response;
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
            InvokeApi<IEnumerable<OrderHeader>> invokeApi = new InvokeApi<IEnumerable<OrderHeader>>(configuration);
            ApiVM<IEnumerable<OrderHeader>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
            IEnumerable<OrderHeader> orders = apiVm.TObject;
            return Json(new { data = orders });
        }
        #endregion
    }
}
