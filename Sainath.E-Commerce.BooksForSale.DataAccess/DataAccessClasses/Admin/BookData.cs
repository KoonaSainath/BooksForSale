using Sainath.E_Commerce.BooksForSale.DataAccess.BaseClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Admin
{
    public class BookData : BaseData
    {
        public BookData(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public IEnumerable<Book> GetAllBooks()
        {
            IQueryable<Book> booksQuery = unitOfWork.BookRepository.GetAllRecords().AsQueryable<Book>();
            return booksQuery.ToList();
        }

        public void InsertBook(Book book)
        {
            unitOfWork.BookRepository.InsertRecord(book);
            unitOfWork.Save();
        }

        public Book GetBook(int id)
        {
            Book book = unitOfWork.BookRepository.GetRecord(id);
            return book;
        }

        public Book GetBook(Expression<Func<Book, bool>> expression)
        {
            Book book = unitOfWork.BookRepository.GetRecord(expression);
            return book;
        }

        public void RemoveBook(Book book)
        {
            unitOfWork.BookRepository.RemoveRecord(book);
            unitOfWork.Save();
        }

        public void RemoveBooks(IEnumerable<Book> books)
        {
            unitOfWork.BookRepository.RemoveRecords(books);
            unitOfWork.Save();
        }

        public void UpdateBook(Book book)
        {
            unitOfWork.BookRepository.UpdateBook(book);
            unitOfWork.Save();
        }
    }
}
