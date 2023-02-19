using Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories.Admin;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.Repositories.Admin
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly BooksForSaleDbContext dbContext;
        public CompanyRepository(BooksForSaleDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateCompany(Company company)
        {
            Company companyFromDb = dbContext.Companies.Where(c => c.CompanyId == company.CompanyId).FirstOrDefault();
            if (companyFromDb != null)
            {
                companyFromDb.CompanyName = company.CompanyName;
                companyFromDb.PhoneNumber = company.PhoneNumber;
                companyFromDb.StreetAddress = company.StreetAddress;
                companyFromDb.City = company.City;
                companyFromDb.State = company.State;
                companyFromDb.PostalCode = company.PostalCode;
                companyFromDb.CreatedDateTime = company.CreatedDateTime;
                companyFromDb.UpdatedDateTime = company.UpdatedDateTime;
            }
        }
    }
}
