using Microsoft.EntityFrameworkCore;
using Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly BooksForSaleDbContext dbContext;
        public BookRepository(BooksForSaleDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateBook(Book book)
        {
            Book bookFromDb = dbContext.Books.FirstOrDefault(x => x.BookId == book.BookId);
            if(bookFromDb != null)
            {
                bookFromDb.Title = book.Title;
                bookFromDb.Description = book.Description;
                bookFromDb.Author = book.Author;
                bookFromDb.ISBN = book.ISBN;
                bookFromDb.ListPrice = book.ListPrice;
                bookFromDb.Price = book.Price;
                bookFromDb.Price50 = book.Price50;
                bookFromDb.Price100 = book.Price100;
                bookFromDb.CategoryId = book.CategoryId;
                bookFromDb.CoverTypeId = book.CoverTypeId;
                if(book.ImageUrl != null)
                {
                    bookFromDb.ImageUrl = book.ImageUrl;
                }
            }
        }
    }
}
