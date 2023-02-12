using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sainath.E_Commerce.BooksForSale.Models.Models
{
    public class BooksForSaleUser : IdentityUser
    {
        [Required(ErrorMessage = "Please enter your name")]
        [Display(Name = "User name")]
        public string Name { get; set; }

        [Display(Name = "Street address")]
        public string? StreetAddress { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        [Display(Name = "Postal code")]
        public string? PostalCode { get; set; }
    }
}
