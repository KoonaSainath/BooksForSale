namespace Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations
{
    public interface IBooksForSaleConfiguration
    {
        string BaseAddressForWebApi { get; set; }
        string StripePublishableKey { get; }
        string StripeSecretKey { get;  }
        string BaseAddressForWebApplication { get; set; }
    }
}
