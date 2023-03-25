using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sainath.E_Commerce.BooksForSale.Web;
using Sainath.E_Commerce.BooksForSale.Web.Areas.Admin.Controllers;
using Sainath.E_Commerce.BooksForSale.Web.Configurations;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.NUnitTest
{
    public class BaseTest<T> where T : Controller
    {
        public Mock<IBooksForSaleConfiguration> mockBooksForSaleConfiguration { get; set; }
        public Mock<IWebHostEnvironment> mockWebHostEnvironment { get; set; }
        public IBooksForSaleConfiguration booksForSaleConfiguration { get; set; }
        public IWebHostEnvironment webHostEnvironment { get; set; }
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
                if (typeof(T) == typeof(BookController))
                {
                    T t = (T) Activator.CreateInstance(typeof(T), booksForSaleConfiguration, webHostEnvironment);
                    return t;
                }
                else
                {
                    T t = (T)Activator.CreateInstance(typeof(T));
                    return t;
                }
            }
        }
    }
}
