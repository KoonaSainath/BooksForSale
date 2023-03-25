using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sainath.E_Commerce.BooksForSale.Web;
using Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers;
using Sainath.E_Commerce.BooksForSale.Web.Areas.Customer.Controllers;
using Sainath.E_Commerce.BooksForSale.Web.Configurations;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Sainath.E_Commerce.BooksForSale.Web.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.NUnitTest
{
    public class BaseTest<T> where T : Controller
    {
        public Mock<IBooksForSaleConfiguration> mockBooksForSaleConfiguration { get; set; }
        public Mock<IWebHostEnvironment> mockWebHostEnvironment { get; set; }
        public Mock<ILogger<T>> mockLogger { get; set; }
        public IBooksForSaleConfiguration booksForSaleConfiguration { get; set; }
        public IWebHostEnvironment webHostEnvironment { get; set; }
        public ILogger<T> logger { get; set; }
        public T UtControllerInstance 
        {
            get
            {
                mockBooksForSaleConfiguration = new Mock<IBooksForSaleConfiguration>();
                mockBooksForSaleConfiguration.SetupProperty(x => x.BaseAddressForWebApi, "https://localhost:7138");
                mockBooksForSaleConfiguration.SetupProperty(x => x.BaseAddressForWebApplication, "https://localhost:7109/");
                this.booksForSaleConfiguration = mockBooksForSaleConfiguration.Object;

                mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
                this.webHostEnvironment = mockWebHostEnvironment.Object;

                mockLogger = new Mock<ILogger<T>>();
                logger = mockLogger.Object;
                T t = null;
                if (typeof(T) == typeof(BookController))
                {
                    t = (T) Activator.CreateInstance(typeof(T), booksForSaleConfiguration, webHostEnvironment);
                }
                else if(typeof(T) == typeof(CategoryController) || typeof(T) == typeof(CompanyController) || typeof(T) == typeof(CoverTypeController) || typeof(T) == typeof(ManageOrdersController))
                {
                    t = (T)Activator.CreateInstance(typeof(T), booksForSaleConfiguration);
                }
                else if(typeof(T) == typeof(HomeController))
                {
                    t = (T)Activator.CreateInstance(typeof(T), logger, booksForSaleConfiguration);
                }
                else
                {
                    t = (T)Activator.CreateInstance(typeof(T));
                }
                return t;
            }
        }
    }
}
