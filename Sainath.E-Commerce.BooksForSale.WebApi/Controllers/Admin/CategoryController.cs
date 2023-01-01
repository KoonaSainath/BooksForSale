using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Admin;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;

namespace Sainath.E_Commerce.BooksForSale.WebApi.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private CategoryDomain categoryDomain;
        public CategoryController()
        {
            categoryDomain = new CategoryDomain();
        }
        [Route(template: "GetAllCategories", Name = "GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            IEnumerable<Category> categories = categoryDomain.GetAllCategories();
            return Ok(categories);
        }
    }
}
