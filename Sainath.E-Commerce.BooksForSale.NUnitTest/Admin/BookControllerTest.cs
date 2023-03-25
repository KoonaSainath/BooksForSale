using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.NUnitTest.Admin
{
    [TestFixture]
    public class BookControllerTest : BaseTest<BookController>
    {
        [Test]
        public void Book_Index()
        {
            ViewResult viewResult = UtControllerInstance.Index() as ViewResult;
            Assert.IsNotNull(viewResult.ViewData);
        }

        [Test]
        public void Book_GetAllBooksApiEndPoint()
        {
            JsonResult jsonResult = UtControllerInstance.GetAllBooksApiEndPoint().GetAwaiter().GetResult() as JsonResult;
            Assert.IsNotNull(jsonResult.Value);
        }
    }
}
