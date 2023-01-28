using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.Models.ViewModels
{
    [Table(name: "Books")]
    public class Book
    {
        [Key]
        [Column(name: "Id")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Please enter book title")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter book description")]
        [MaxLength(1000)]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Please enter ISBN number")]
        [MaxLength(20)]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Please enter author name")]
        [MaxLength(50)]
        public string Author { get; set; }

        [Required(ErrorMessage = "Please enter list price of the book")]
        [Range(minimum: 1, maximum: 10000, ErrorMessage = "List price has to be between 1 and 10,000 included")]
        [Display(Name = "List price")]
        public double? ListPrice { get; set; }

        [Required(ErrorMessage = "Please enter price of the book")]
        [Range(minimum: 1, maximum: 10000, ErrorMessage = "Price has to be between 1 and 10,000 included")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Please enter price of each book if 50 or more number of books are ordered")]
        [Range(minimum: 1, maximum: 10000, ErrorMessage = "Price has to be between 1 and 10,000 included")]
        [Display(Name = "Price of each book if 50 or more number of books are ordered")]
        public double? Price50 { get; set; }

        [Required(ErrorMessage = "Please enter price of each book if 100 or more number of books are ordered")]
        [Range(minimum: 1, maximum: 10000, ErrorMessage = "Price has to be between 1 and 10,000 included")]
        [Display(Name = "Price of each book if 100 or more number of books are ordered")]
        public double? Price100 { get; set; }

        [ValidateNever]
        [Display(Name = "Upload an image of the book")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Book category")]
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Please select a cover type")]
        [Display(Name = "Book cover type")]
        public int? CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId")]
        [ValidateNever]
        public CoverType CoverType { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
    }
}
