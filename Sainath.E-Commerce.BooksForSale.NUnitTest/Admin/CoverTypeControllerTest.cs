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
    public class CoverTypeControllerTest : BaseTest<CoverTypeController>
    {
        [Test]
        public void CoverType_Index()
        {
            ViewResult viewResult = UtControllerInstance.Index().GetAwaiter().GetResult() as ViewResult;
            Assert.IsNotNull(viewResult.ViewData);
        }
        [Test]
        public void CoverType_InsertCoverType()
        {
            ViewResult viewResult = UtControllerInstance.InsertCoverType().GetAwaiter().GetResult() as ViewResult;
            Assert.IsNotNull(viewResult.ViewData);
        }
        [TestCase(0)]
        [TestCase(27)]
        [TestCase(28)]
        public void CoverType_UpdateCoverType(int? coverTypeId)
        {
            if(coverTypeId.GetValueOrDefault() == 0)
            {
                NotFoundResult notFoundResult = UtControllerInstance.UpdateCoverType(coverTypeId).GetAwaiter().GetResult() as NotFoundResult;
                Assert.IsNotNull(notFoundResult);
            }
            else
            {
                ViewResult viewResult = UtControllerInstance.UpdateCoverType(coverTypeId).GetAwaiter().GetResult() as ViewResult;
                Assert.IsNotNull(viewResult.ViewData);
            }
        }
        [TestCase(0)]
        [TestCase(27)]
        [TestCase(28)]
        public void CoverType_RemoveCoverType(int? coverTypeId)
        {
            if (coverTypeId.GetValueOrDefault() == 0)
            {
                NotFoundResult notFoundResult = UtControllerInstance.RemoveCoverType(coverTypeId).GetAwaiter().GetResult() as NotFoundResult;
                Assert.IsNotNull(notFoundResult);
            }
            else
            {
                ViewResult viewResult = UtControllerInstance.RemoveCoverType(coverTypeId).GetAwaiter().GetResult() as ViewResult;
                Assert.IsNotNull(viewResult.ViewData);
            }
        }
        [Test]
        public void CoverType_GetAllCoverTypesApiEndPoint()
        {
            JsonResult jsonResult = UtControllerInstance.GetAllCoverTypesApiEndPoint().GetAwaiter().GetResult() as JsonResult;
            Assert.IsNotNull(jsonResult);
        }

    }
}
