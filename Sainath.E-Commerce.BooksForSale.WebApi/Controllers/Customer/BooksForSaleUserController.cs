using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Customer;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;

namespace Sainath.E_Commerce.BooksForSale.WebApi.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksForSaleUserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly BooksForSaleUserDomain booksForSaleUserDomain;
        public BooksForSaleUserController(IUnitOfWork unitOfWork)
        {
            booksForSaleUserDomain = new BooksForSaleUserDomain(unitOfWork);
        }

        [HttpGet]
        [Route(template: "GET/GetUser/{userId}", Name = "GetUser")]
        public IActionResult GetUser(string userId)
        {
            BooksForSaleUser user = booksForSaleUserDomain.GetUser(userId);
            return Ok(user);
        }
    }
}
