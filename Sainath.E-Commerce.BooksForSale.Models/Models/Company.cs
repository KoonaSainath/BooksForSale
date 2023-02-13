using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sainath.E_Commerce.BooksForSale.Models.Models
{
    [Table(name: "Companies")]
    public class Company
    {
        [Key]
        [Column(name: "Id")]
        public int CompanyId { get; set; }

        [Display(Name = "Company name")]
        [Required(ErrorMessage = "Please enter company name")]
        [Range(minimum: 2, maximum: 100, ErrorMessage = "Company name must contain atleast 2 and atmost 100 number of characters")]
        public string CompanyName { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Please enter phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Street address")]
        [Required(ErrorMessage = "Please enter street address")]
        [Range(minimum: 10, maximum: 500, ErrorMessage = "Street address must contain atleast 10 and atmost 500 number of characters")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Please enter city")]
        [Range(minimum: 2, maximum: 50, ErrorMessage = "City must contain atleast 2 and atmost 50 number of characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter state")]
        [Range(minimum: 2, maximum: 50, ErrorMessage = "State must contain atleast 2 and atmost 50 number of characters")]
        public string State { get; set; }

        [Display(Name = "Postal code")]
        [Required(ErrorMessage = "Please enter postal code")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
    }
}
