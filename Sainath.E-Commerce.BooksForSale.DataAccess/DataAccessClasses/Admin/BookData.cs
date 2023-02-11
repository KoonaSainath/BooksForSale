using Sainath.E_Commerce.BooksForSale.DataAccess.BaseClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models;
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
        private readonly CategoryData categoryData;
        private readonly CoverTypeData coverTypeData;
        public BookData(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            categoryData = new CategoryData(unitOfWork);
            coverTypeData = new CoverTypeData(unitOfWork);
        }

        public IEnumerable<Book> GetAllBooks(string includeProperties)
        {
            return unitOfWork.BookRepository.GetAllRecords(includeProperties).OrderByDescending(book => book.BookId);
        }

        public void InsertBook(Book book)
        {
            unitOfWork.BookRepository.InsertRecord(book);
            unitOfWork.Save();
        }

        public Book GetBook(int id)
        {
            Book book = unitOfWork.BookRepository.GetRecord(id);
            book.Category = categoryData.GetCategory((int)book.CategoryId);
            book.CoverType = coverTypeData.GetCoverTypeById((int)book.CoverTypeId);
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
