using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models;

namespace Sainath.E_Commerce.BooksForSale.WebApi.Controllers.Admin
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyDomain companyDomain;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            companyDomain = new CompanyDomain(unitOfWork);
        }

        [HttpGet]
        [Route(template: "GET/GetAllCompanies", Name = "GetAllCompanies")]
        public IActionResult GetAllCompanies()
        {
            IEnumerable<Company> companies = companyDomain.GetAllCompanies();
            return Ok(companies);
        }

        [HttpGet]
        [Route(template: "GET/GetCompany/{companyId}", Name = "GetCompany")]
        public IActionResult GetCompany(int companyId)
        {
            Company company = companyDomain.GetCompany(companyId);
            return Ok(company);
        }

        [HttpPost]
        [Route(template: "POST/InsertCompany", Name = "InsertCompany")]
        public IActionResult InsertCompany(Company company)
        {
            companyDomain.InsertCompany(company);
            return Ok("Company inserted successfully");
        }

        [HttpPost]
        [Route(template: "DELETE/RemoveCompany", Name = "RemoveCompany")]
        public IActionResult RemoveCompany(Company company)
        {
            companyDomain.RemoveCompany(company);
            return Ok("Company removed successfully");
        }

        [HttpPost]
        [Route(template: "DELETE/RemoveCompanies", Name = "RemoveCompanies")]
        public IActionResult RemoveCompanies(IEnumerable<Company> companies)
        {
            companyDomain.RemoveCompanies(companies);
            return Ok("Companies removed successfully");
        }

        [HttpPut]
        [Route(template: "PUT/UpdateCompany", Name = "UpdateCompany")]
        public IActionResult Updatecompany(Company company)
        {
            companyDomain.UpdateCompany(company);
            return Ok("Company updated successfully");
        }
    }
}
