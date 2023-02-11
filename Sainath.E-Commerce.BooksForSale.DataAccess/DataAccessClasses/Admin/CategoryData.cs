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
    public class CategoryData : BaseData
    {
        public CategoryData(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {

        }  
        public IEnumerable<Category> GetAllCategories()
        {
            return unitOfWork.CategoryRepository.GetAllRecords().OrderByDescending(category => category.CategoryId);
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
