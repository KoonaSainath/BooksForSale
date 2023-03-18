using Microsoft.AspNetCore.Mvc;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using System.Net.Http.Headers;

namespace Sainath.E_Commerce.BooksForSale.Web.HelperClasses
{
    public class InvokeApi<T> where T : class
    {
        private readonly IBooksForSaleConfiguration configuration;
        public InvokeApi(IBooksForSaleConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<ApiVM<T>> Invoke(string requestUrl, HttpMethod httpMethod, T? tObject = null)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(configuration.BaseAddressForWebApi);

            ApiVM<T> apiVm = new ApiVM<T>();

            if(httpMethod == HttpMethod.Get)
            {
                T result = await httpClient.GetFromJsonAsync<T>(requestUrl);
                apiVm.TObject = result;
            }
            else if(httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Delete)
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<T>(requestUrl, tObject);
                apiVm.Response = response;
            }
            else if(httpMethod == HttpMethod.Put)
            {
                HttpResponseMessage response = await httpClient.PutAsJsonAsync<T>(requestUrl, tObject);
                apiVm.Response = response;
            }
            return apiVm;
        }
    }
}
