﻿using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;

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
        public string StripePublishableKey
        {
            get
            {
                return configuration["StripeKeys:StripePublishableKey"];
            }
        }
        public string StripeSecretKey
        {
            get
            {
                return configuration["StripeKeys:StripeSecretKey"];
            }
        }
        private readonly IConfiguration configuration;
        public BooksForSaleConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
