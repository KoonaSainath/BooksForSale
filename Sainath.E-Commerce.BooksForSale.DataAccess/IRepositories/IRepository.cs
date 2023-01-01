using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAllRecords();
        T GetRecord(int id);
        T GetRecord(Expression<Func<T, bool>> expression);
        void InsertRecord(T record);
        void RemoveRecord(T record);
        void RemoveRecords(IEnumerable<T> records);
    }
}
