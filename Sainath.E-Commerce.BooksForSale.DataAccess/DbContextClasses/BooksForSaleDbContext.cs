using Microsoft.EntityFrameworkCore;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses
{
    public class BooksForSaleDbContext : DbContext 
    {
        public BooksForSaleDbContext(DbContextOptions<BooksForSaleDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
