using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;

namespace Sainath.E_Commerce.BooksForSale.Web.Configurations
{
    public class BooksForSaleConfiguration : IBooksForSaleConfiguration
    {
        public string BaseAddressForWebApi
        {
            get
            {
                return configuration["BaseAddresses:BaseAddressForWebApi"];
            }
        }
        private readonly IConfiguration configuration;
        public BooksForSaleConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
