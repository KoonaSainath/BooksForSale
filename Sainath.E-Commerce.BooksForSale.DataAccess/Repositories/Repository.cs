using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BooksForSaleDbContext dbContext;
        private DbSet<T> dbSet;
        public Repository(BooksForSaleDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }
        public IEnumerable<T> GetAllRecords()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }
        public void InsertRecord(T record)
        {
            dbSet.Add(record);
        }
        public T GetRecord(int id)
        {
            T record = dbSet.Find(id);
            return record;
        }
        public T GetRecord(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> query = dbSet;
            T record = query.FirstOrDefault();
            return record;
        }
        public void RemoveRecord(T record)
        {
            dbSet.Remove(record);
        }
        public void RemoveRecords(IEnumerable<T> records)
        {
            dbSet.RemoveRange(records);
        }
    }
}
