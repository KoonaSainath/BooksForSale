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
    public class CompanyControllerTest : BaseTest<CompanyController>
    {
        [Test]
        public void Company_Index()
        {
            ViewResult viewResult = UtControllerInstance.Index() as ViewResult;
            Assert.IsNotNull(viewResult.ViewData);
        }
        [TestCase(0)]
        [TestCase(4)]
        [TestCase(5)]
        public void Company_UpsertCompany(int? companyId)
        {
            ViewResult viewResult = UtControllerInstance.UpsertCompany(companyId).GetAwaiter().GetResult() as ViewResult;
            Assert.IsNotNull(viewResult.ViewData);
        }
        [Test]
        public void Company_GetAllCompanies()
        {
            JsonResult viewResult = UtControllerInstance.GetAllCompanies().GetAwaiter().GetResult() as JsonResult;
            Assert.IsNotNull(viewResult.Value);
        }
    }
}
