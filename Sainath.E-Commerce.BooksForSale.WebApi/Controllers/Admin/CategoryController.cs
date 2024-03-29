﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using System.Linq.Expressions;

namespace Sainath.E_Commerce.BooksForSale.WebApi.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryDomain categoryDomain;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            categoryDomain = new CategoryDomain(unitOfWork);
        }
        [HttpGet]
        [Route(template: "GET/GetAllCategories", Name = "GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            IEnumerable<Category> categories = categoryDomain.GetAllCategories();
            return Ok(categories);
        }
        [HttpPost]
        [Route(template: "POST/InsertCategory", Name = "InsertCategory")]
        public IActionResult InsertCategory(Category category)
        {
            Category insertedCategory = categoryDomain.InsertCategory(category);
            return Ok(insertedCategory);
        }
        [HttpGet]
        [Route(template: "GET/GetCategoryById/{id}", Name = "GetCategoryById")]
        public IActionResult GetCategoryById(int id)
        {
            Category category = categoryDomain.GetCategory(id);
            return Ok(category);
        }
        [HttpPost]
        [Route(template: "DELETE/RemoveCategory", Name = "RemoveCategory")]
        public IActionResult RemoveCategory(Category category)
        {
            categoryDomain.RemoveCategory(category);
            return Ok("Category deleted successfully!");
        }
        [HttpPost]
        [Route(template: "DELETE/RemoveCategories", Name = "RemoveCategories")]
        public IActionResult RemoveCategories(IEnumerable<Category> categories)
        {
            categoryDomain.RemoveCategories(categories);
            return Ok("Categories deleted successfully!");
        }
        [HttpPut]
        [Route(template: "PUT/UpdateCategory", Name = "UpdateCategory")]
        public IActionResult UpdateCategory(Category category)
        {
            Category updatedCategory = categoryDomain.UpdateCategory(category);
            return Ok(updatedCategory);
        }
    }
}
