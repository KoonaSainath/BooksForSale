using Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Admin
{
    public class BookDomain
    {
        private readonly BookData bookData;
        public BookDomain(IUnitOfWork unitOfWork)
        {
            bookData = new BookData(unitOfWork);
        }
        public IEnumerable<Book> GetAllBooks(string includeProperties)
        {
            IEnumerable<Book> allBooks = bookData.GetAllBooks(includeProperties);
            return allBooks;
        }
        public Book GetBook(int id)
        {
            Book book = bookData.GetBook(id);
            return book;
        }
        public Book GetBook(Expression<Func<Book, bool>> expression)
        {
            Book book = bookData.GetBook(expression);
            return book;
        }
        public void InsertBook(Book book)
        {
            bookData.InsertBook(book);
        }
        public void RemoveBook(Book book)
        {
            bookData.RemoveBook(book);
        }
        public void RemoveBooks(IEnumerable<Book> books)
        {
            bookData.RemoveBooks(books);
        }
        public Book UpdateBook(Book book)
        {
            bookData.UpdateBook(book);
            return book;
        }
    }
}
