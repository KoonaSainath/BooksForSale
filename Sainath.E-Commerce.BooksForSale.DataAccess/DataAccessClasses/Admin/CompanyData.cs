using Sainath.E_Commerce.BooksForSale.DataAccess.BaseClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Admin
{
    public class CompanyData : BaseData
    {
        public CompanyData(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        
        public IEnumerable<Company> GetAllCompanies()
        {
            return unitOfWork.CompanyRepository.GetAllRecords().OrderByDescending(company => company.CompanyId);
        }

        public Company GetCompany(int companyId)
        {
            Company company = unitOfWork.CompanyRepository.GetRecord(companyId);
            return company;
        }

        public void InsertCompany(Company company)
        {
            unitOfWork.CompanyRepository.InsertRecord(company);
            unitOfWork.Save();
        }

        public void RemoveCompany(Company company)
        {
            unitOfWork.CompanyRepository.RemoveRecord(company);
            unitOfWork.Save();
        }

        public void RemoveCompanies(IEnumerable<Company> companies)
        {
            unitOfWork.CompanyRepository.RemoveRecords(companies);
            unitOfWork.Save();
        }

        public void UpdateCompany(Company company)
        {
            unitOfWork.CompanyRepository.UpdateCompany(company);
            unitOfWork.Save();
        }
    }
}
