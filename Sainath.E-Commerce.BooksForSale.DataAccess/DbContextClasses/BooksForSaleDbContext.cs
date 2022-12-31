﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses
{
    public class BooksForSaleDbContext : DbContext 
    {
        public BooksForSaleDbContext(DbContextOptions<BooksForSaleDbContext> options) : base(options)
        {
        }
    }
}