using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sainath.E_Commerce.BooksForSale.Models.ViewModels
{
    [Table(name: "Categories")]
    public class Category
    {
        [Key]
        [Column(name: "Id")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please enter category name.")]
        [MaxLength(50)]
        [Column(name: "Name")]
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Please enter display order.")]
        [Range(minimum: 1, maximum: 100, ErrorMessage = "Display order must be between 1 and 100, both included.")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
    }
}
