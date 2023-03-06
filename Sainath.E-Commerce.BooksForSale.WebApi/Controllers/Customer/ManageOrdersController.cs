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
        [Route(template: "GET/GetAllOrders/{userId}/{isUserAdminOrEmployee}/{includeProperties?}", Name = "GetAllOrders")]
        public IActionResult GetAllOrders(string userId, bool isUserAdminOrEmployee, string? includeProperties = null)
        {
            IEnumerable<OrderHeader> orders = manageOrdersDomain.GetAllOrders(userId, isUserAdminOrEmployee, includeProperties);
            return Ok(orders);
        }
    }
}
