using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Areas.Customer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.NUnitTest.Customer
{
    [TestFixture]
    public class ManageOrdersControllerTest : BaseTest<ManageOrdersController>
    {
        [TestCase(null)]
        [TestCase(OrderStatus.STATUS_APPROVED)]
        public void ManageOrders_Index(string? status)
        {
            ViewResult viewResult = UtControllerInstance.Index(status) as ViewResult;
            Assert.IsNotNull(viewResult.ViewData);
        }
        [TestCase(34)]
        [TestCase(35)]
        public void ManageOrders_GetOrder(int orderHeaderId)
        {
            ViewResult viewResult = UtControllerInstance.GetOrder(orderHeaderId).GetAwaiter().GetResult() as ViewResult;
            Assert.IsNotNull(viewResult.ViewData);
        }
    }
}
