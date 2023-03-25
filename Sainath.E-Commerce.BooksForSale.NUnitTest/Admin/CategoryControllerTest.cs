using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.NUnitTest.Admin
{
    [TestFixture]
    public class CategoryControllerTest : BaseTest<CategoryController>
    {
        [Test]
        public void Category_Index()
        {
            ViewResult viewResult = UtControllerInstance.Index().GetAwaiter().GetResult() as ViewResult;
            Assert.IsNotNull(viewResult.ViewData);
        }
        [Test]
        public void Category_InsertCategory()
        {
            ViewResult viewResult = UtControllerInstance.InsertCategory().GetAwaiter().GetResult() as ViewResult;
            Assert.IsNotNull(viewResult.ViewData);
        }
        [TestCase(0)]
        [TestCase(16)]
        [TestCase(17)]
        [TestCase(18)]
        public void Category_UpdateCategory(int? categoryId)
        {
            if(categoryId.GetValueOrDefault() == 0)
            {
                NotFoundResult notFoundResult = UtControllerInstance.UpdateCategory(categoryId).GetAwaiter().GetResult() as NotFoundResult;
                Assert.IsNotNull(notFoundResult);
            }
            else
            {
                ViewResult viewResult = UtControllerInstance.UpdateCategory(categoryId).GetAwaiter().GetResult() as ViewResult;
                Assert.IsNotNull(viewResult.ViewData);
            }
        }
        [Test]
        public void Category_GetAllCategoriesApiEndPoint()
        {
            JsonResult jsonResult = UtControllerInstance.GetAllCategoriesApiEndPoint().GetAwaiter().GetResult() as JsonResult;
            Assert.IsNotNull(jsonResult.Value);
        }
    }
}
