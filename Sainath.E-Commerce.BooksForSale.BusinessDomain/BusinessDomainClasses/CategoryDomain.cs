﻿using Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses
{
    public class CategoryDomain
    {
        private CategoryData categoryData;
        public CategoryDomain()
        {
            categoryData = new CategoryData();
        }
        public IEnumerable<Category> GetAllCategories()
        {
            return categoryData.GetAllCategories();
        }
        public void InsertCategory(Category category)
        {
            categoryData.InsertCategory(category);
        }
        public Category GetCategory(int id)
        {
            Category category = categoryData.GetCategory(id);
            return category;
        }
        public Category GetCategory(Expression<Func<Category, bool>> expression)
        {
            Category category = categoryData.GetCategory(expression);
            return category;
        }
        public void RemoveCategory(Category category)
        {
            categoryData.RemoveCategory(category);
        }
        public void RemoveCategories(IEnumerable<Category> categories)
        {
            categoryData.RemoveCategories(categories);
        }
    }
}