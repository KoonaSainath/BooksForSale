using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Sainath.E_Commerce.BooksForSale.Web.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly IBooksForSaleConfiguration configuration;
        public CartCountViewComponent(IBooksForSaleConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);
                string userId = claim.Value;
                string requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{userId}";
                IEnumerable<ShoppingCart> shoppingCarts = await httpClient.GetFromJsonAsync<IEnumerable<ShoppingCart>>(requestUrl);
                if(HttpContext.Session.GetInt32(GenericConstants.CartCountKey).GetValueOrDefault() == 0)
                {
                    HttpContext.Session.SetInt32(GenericConstants.CartCountKey, shoppingCarts.Count());
                    return View(HttpContext.Session.GetInt32(GenericConstants.CartCountKey));
                }
                else
                {
                    return View(HttpContext.Session.GetInt32(GenericConstants.CartCountKey));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
