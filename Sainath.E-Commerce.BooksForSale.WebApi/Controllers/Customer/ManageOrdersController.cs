using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Customer;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;

namespace Sainath.E_Commerce.BooksForSale.WebApi.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageOrdersController : ControllerBase
    {
        private readonly ManageOrdersDomain manageOrdersDomain;
        public ManageOrdersController(IUnitOfWork unitOfWork)
        {
            manageOrdersDomain = new ManageOrdersDomain(unitOfWork);
        }

        [HttpGet]
        [Route(template: "GET/GetAllOrders/{userId}/{isUserAdminOrEmployee}/{status}/{includeProperties?}", Name = "GetAllOrders")]
        public IActionResult GetAllOrders(string userId, bool isUserAdminOrEmployee, string status, string? includeProperties = null)
        {
            IEnumerable<OrderHeader> orders = manageOrdersDomain.GetAllOrders(userId, isUserAdminOrEmployee, status, includeProperties);
            return Ok(orders);
        }

        [HttpGet]
        [Route(template: "GET/GetOrder/{orderHeaderId}/{includeProperties?}", Name = "GetOrder")]
        public IActionResult GetOrder(int orderHeaderId, string? includeProperties = null)
        {
            OrderHeader orderHeader = manageOrdersDomain.GetOrder(orderHeaderId, includeProperties);
            return Ok(orderHeader);
        }

        [HttpGet]
        [Route(template: "GET/GetOrderDetails/{orderHeaderId}/{includeProperties?}", Name = "GetOrderDetails")]
        public IActionResult GetOrderDetails(int orderHeaderId, string? includeProperties = null)
        {
            IEnumerable<OrderDetails> orderDetails = manageOrdersDomain.GetOrderDetails(orderHeaderId, includeProperties);
            return Ok(orderDetails);
        }

        [HttpPut]
        [Route(template: "PUT/UpdateOrder", Name = "UpdateOrder")]
        public IActionResult UpdateOrder(OrderHeader orderHeader)
        {
            manageOrdersDomain.UpdateOrder(orderHeader);
            return Ok("Order updated successfully!");
        }
    }
}
