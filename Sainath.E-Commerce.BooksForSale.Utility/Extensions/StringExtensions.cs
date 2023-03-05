using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.Utility.Extensions
{
    public static class StringExtensions
    {
        public static string NullCheckTrim(this string stringValue)
        {
            stringValue = (string.IsNullOrEmpty(stringValue) ? string.Empty : stringValue);
            return stringValue.Trim();
        }
    }
}
