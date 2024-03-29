﻿using Microsoft.Identity.Client;
using Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories.Admin;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.Repositories.Admin
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly BooksForSaleDbContext dbContext;
        public CategoryRepository(BooksForSaleDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public void UpdateCategory(Category category)
        {
            dbContext.Categories.Update(category);
        }
    }
}
