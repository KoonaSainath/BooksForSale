using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;

namespace Sainath.E_Commerce.BooksForSale.WebApi.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoverTypeController : ControllerBase
    {
        private readonly CoverTypeDomain coverTypeDomain;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            coverTypeDomain = new CoverTypeDomain(unitOfWork);
        }
        [HttpGet]
        [Route(template: "GET/GetAllCoverTypes", Name = "GetAllCoverTypes")]
        public IActionResult GetAllCoverTypes()
        {
            IEnumerable<CoverType> coverTypes = coverTypeDomain.GetAllCoverTypes();
            return Ok(coverTypes);
        }
        [HttpPost]
        [Route(template: "POST/InsertCoverType", Name = "InsertCoverType")]
        public IActionResult InsertCoverType(CoverType coverType)
        {
            CoverType insertedCoverType = coverTypeDomain.InsertCoverType(coverType);
            return Ok(insertedCoverType);
        }
        [HttpGet]
        [Route(template: "GET/GetCoverType/{coverTypeId}", Name = "GetCoverType")]
        public IActionResult GetCoverType(int coverTypeId)
        {
            CoverType coverType = coverTypeDomain.GetCoverType(coverTypeId);
            return Ok(coverType);
        }
        [HttpPost]
        [Route(template: "DELETE/RemoveCoverType", Name = "RemoveCoverType")]
        public IActionResult RemoveCoverType(CoverType coverType)
        {
            coverTypeDomain.RemoveCoverType(coverType);
            return Ok("Cover type removed successfully!");
        }
        [HttpPost]
        [Route(template: "DELETE/RemoveCoverTypes", Name = "RemoveCoverTypes")]
        public IActionResult RemoveCoverTypes(IEnumerable<CoverType> coverTypes)
        {
            coverTypeDomain.RemoveCoverTypes(coverTypes);
            return Ok("Cover types removed successfully!");
        }
        [HttpPut]
        [Route(template: "PUT/UpdateCoverType", Name = "UpdateCoverType")]
        public IActionResult UpdateCoverType(CoverType coverType)
        {
            CoverType updatedCoverType = coverTypeDomain.UpdateCoverType(coverType);
            return Ok(updatedCoverType);
        }
    }

}
