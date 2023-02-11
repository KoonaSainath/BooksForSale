using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.Models.Models
{
    public class ShoppingCart
    {
        public Book Book { get; set; }

        [Required(ErrorMessage = "Please enter number of books you want to add to the shopping cart")]
        [Range(minimum: 1, maximum: 100, ErrorMessage = "Only 1 to 100 number of books can be added to the shopping cart")]
        [Display(Name = "Number of books")]
        public int? CartItemCount { get; set; }
    }
}
