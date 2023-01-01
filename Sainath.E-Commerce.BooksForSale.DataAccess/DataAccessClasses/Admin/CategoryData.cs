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
    public class CategoryData : BaseData
    {
        public IEnumerable<Category> GetAllCategories()
        {
            return unitOfWork.CategoryRepository.GetAllRecords();
        }
        public void InsertCategory(Category category)
        {
            unitOfWork.CategoryRepository.InsertRecord(category);
            unitOfWork.Save();
        }
        public Category GetCategory(int id)
        {
            return unitOfWork.CategoryRepository.GetRecord(id);
        }
        public Category GetCategory(Expression<Func<Category, bool>> expression)
        {
            return unitOfWork.CategoryRepository.GetRecord(expression);
        }
        public void RemoveCategory(Category category)
        {
            unitOfWork.CategoryRepository.RemoveRecord(category);
            unitOfWork.Save();
        }
        public void RemoveCategories(IEnumerable<Category> categories)
        {
            unitOfWork.CategoryRepository.RemoveRecords(categories);
            unitOfWork.Save();
        }
        public Category UpdateCategory(Category category)
        {
            unitOfWork.CategoryRepository.UpdateCategory(category);
            unitOfWork.Save();
            return category;
        }
    }
}
