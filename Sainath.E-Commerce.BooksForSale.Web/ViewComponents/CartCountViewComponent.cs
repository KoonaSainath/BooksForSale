using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Models.Models.Customer;
using Sainath.E_Commerce.BooksForSale.Utility.Constants;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Sainath.E_Commerce.BooksForSale.Web.HelperClasses;
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
                string userId = claim.Value;
                string requestUrl = $"api/ShoppingCart/GET/GetAllShoppingCarts/{userId}";
                InvokeApi<IEnumerable<ShoppingCart>> invokeApi = new InvokeApi<IEnumerable<ShoppingCart>>(configuration);
                ApiVM<IEnumerable<ShoppingCart>> apiVm = await invokeApi.Invoke(requestUrl, HttpMethod.Get);
                IEnumerable<ShoppingCart> shoppingCarts = apiVm.TObject;
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
