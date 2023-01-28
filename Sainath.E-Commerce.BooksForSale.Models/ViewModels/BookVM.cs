using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.Models.ViewModels
{
    public class BookVM
    {
        public Book Book { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? Categories { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CoverTypes { get; set; }
    }
}
