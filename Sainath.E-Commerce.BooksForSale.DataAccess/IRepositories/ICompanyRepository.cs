﻿using Sainath.E_Commerce.BooksForSale.DataAccess.Repositories;
using Sainath.E_Commerce.BooksForSale.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public void UpdateCompany(Company company);
    }
}
