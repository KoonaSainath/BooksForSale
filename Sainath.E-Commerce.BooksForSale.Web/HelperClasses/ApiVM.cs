using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sainath.E_Commerce.BooksForSale.Web.HelperClasses
{
    public class ApiVM<T> where T : class
    {
        public T TObject { get; set; }
        public HttpResponseMessage Response { get; set; }
    }
}
