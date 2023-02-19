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
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly BooksForSaleDbContext dbContext;
        public CoverTypeRepository(BooksForSaleDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public void UpdateCoverType(CoverType coverType)
        {
            dbContext.Update(coverType);
        }
    }
}
