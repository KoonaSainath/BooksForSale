using Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses
{
    public class CompanyDomain
    {
        private readonly CompanyData companyData;
        public CompanyDomain(IUnitOfWork unitOfWork)
        {
            companyData = new CompanyData(unitOfWork);
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            return companyData.GetAllCompanies();
        }

        public Company GetCompany(int companyId)
        {
            return companyData.GetCompany(companyId);
        }

        public void InsertCompany(Company company)
        {
            companyData.InsertCompany(company);
        }

        public void RemoveCompany(Company company)
        {
            companyData.RemoveCompany(company);
        }

        public void RemoveCompanies(IEnumerable<Company> companies)
        {
            companyData.RemoveCompanies(companies);
        }

        public void UpdateCompany(Company company)
        {
            companyData.UpdateCompany(company);
        }
    }
}
