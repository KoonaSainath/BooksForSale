using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;

namespace Sainath.E_Commerce.BooksForSale.WebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookDomain bookDomain;
        public BookController(IUnitOfWork unitOfWork)
        {
            bookDomain = new BookDomain(unitOfWork);
        }
        [HttpGet]
        [Route(template: "GET/GetAllBooks/{includeProperties}", Name = "GetAllBooks")]
        public IActionResult GetAllBooks(string includeProperties)
        {
            IEnumerable<Book> allBooks = bookDomain.GetAllBooks(includeProperties);
            return Ok(allBooks);
        }
        [HttpGet]
        [Route(template: "GET/GetBook/{id}", Name = "GetBook")]
        public IActionResult GetBook(int id)
        {
            Book book = bookDomain.GetBook(id);
            return Ok(book);
        }
        [HttpPost]
        [Route(template: "POST/InsertBook", Name = "InsertBook")]
        public IActionResult InsertBook(Book book)
        {
            bookDomain.InsertBook(book);
            return Ok("Book inserted successfully!");
        }
        [HttpPost]
        [Route(template: "DELETE/RemoveBook", Name = "RemoteBook")]
        public IActionResult RemoveBook(Book book)
        {
            bookDomain.RemoveBook(book);
            return Ok("Book removed successfully!");
        }
        [HttpPost]
        [Route(template: "DELETE/RemoveBooks", Name = "RemoveBooks")]
        public IActionResult RemoveBooks(IEnumerable<Book> books)
        {
            bookDomain.RemoveBooks(books);
            return Ok("Books removed successfully!");
        }
        [HttpPut]
        [Route(template: "PUT/UpdateBook", Name = "UpdateBook")]
        public IActionResult UpdateBook(Book book)
        {
            Book updatedBook = bookDomain.UpdateBook(book);
            return Ok(updatedBook);
        }
    }
}
