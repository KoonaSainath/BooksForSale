using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAllRecords(string includeProperties = null, Expression<Func<T,bool>> expression = null);
        T GetRecord(int id);
        void InsertRecord(T record);
        void RemoveRecord(T record);
        void RemoveRecords(IEnumerable<T> records);
        T GetRecordByExpression(Expression<Func<T, bool>> expression, string includeProperties = null);
    }
}
