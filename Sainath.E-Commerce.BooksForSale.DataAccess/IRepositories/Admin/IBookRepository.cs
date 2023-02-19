using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories.Admin
{
    public interface IBookRepository : IRepository<Book>
    {
        void UpdateBook(Book book);
    }
}
