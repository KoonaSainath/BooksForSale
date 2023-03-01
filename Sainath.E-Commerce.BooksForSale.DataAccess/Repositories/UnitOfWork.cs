using Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories.Customer;
using Sainath.E_Commerce.BooksForSale.DataAccess.Repositories.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.Repositories.Customer;
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
        public ICoverTypeRepository CoverTypeRepository { get; private set; }
        public IBookRepository BookRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public IBooksForSaleUserRepository BooksForSaleUserRepository { get; private set; }
        public IOrderHeaderRepository OrderHeaderRepository { get; private set; }
        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }
        private readonly BooksForSaleDbContext dbContext;
        public UnitOfWork(BooksForSaleDbContext dbContext)
        {
            this.dbContext = dbContext;
            CategoryRepository = new CategoryRepository(dbContext);
            CoverTypeRepository = new CoverTypeRepository(dbContext);
            BookRepository = new BookRepository(dbContext);
            CompanyRepository = new CompanyRepository(dbContext);
            ShoppingCartRepository = new ShoppingCartRepository(dbContext);
            BooksForSaleUserRepository = new BooksForSaleUserRepository(dbContext);
            OrderHeaderRepository = new OrderHeaderRepository(dbContext);
            OrderDetailsRepository = new OrderDetailsRepository(dbContext);
        }
        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
