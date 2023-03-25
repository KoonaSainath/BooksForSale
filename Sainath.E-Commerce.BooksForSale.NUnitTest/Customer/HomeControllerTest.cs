using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Web.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.NUnitTest.Customer
{
    [TestFixture]
    public class HomeControllerTest : BaseTest<HomeController>
    {
        [Test]
        public void Home_Index()
        {
            ViewResult viewResult = UtControllerInstance.Index().GetAwaiter().GetResult() as ViewResult;
            Assert.IsNotNull(viewResult);
        }
        [Test]
        public void Home_Privacy()
        {
            ViewResult viewResult = UtControllerInstance.Privacy() as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}
