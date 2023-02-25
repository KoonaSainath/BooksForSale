using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sainath.E_Commerce.BooksForSale.Models.Models.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.Models.Models.Customer
{
    [Table(name: "OrderDetails")]
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        [ValidateNever]
        public Book Book { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public double OrderPrice { get; set; }
    }
}
