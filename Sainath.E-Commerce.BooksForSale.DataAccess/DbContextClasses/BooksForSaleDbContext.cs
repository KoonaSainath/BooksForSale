using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sainath.E_Commerce.BooksForSale.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses
{
    public class BooksForSaleDbContext : IdentityDbContext 
    {
        public BooksForSaleDbContext(DbContextOptions<BooksForSaleDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BooksForSaleUser> BooksForSaleUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}
