using Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; private set; }
        private readonly BooksForSaleDbContext dbContext;
        public UnitOfWork(BooksForSaleDbContext dbContext)
        {
            this.dbContext = dbContext;
            CategoryRepository = new CategoryRepository(dbContext);
        }
        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
