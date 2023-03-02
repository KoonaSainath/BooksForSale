namespace Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations
{
    public interface IBooksForSaleConfiguration
    {
        string BaseAddressForWebApi { get; }
        string StripePublishableKey { get; }
        string StripeSecretKey { get;  }
    }
}
