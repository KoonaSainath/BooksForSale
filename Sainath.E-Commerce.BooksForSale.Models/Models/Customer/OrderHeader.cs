using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.Models.Models.Customer
{
    [Table(name: "OrderHeaders")]
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public BooksForSaleUser BooksForSaleUser { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        [Required]
        public double TotalOrderAmount { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string? StripeSessionId { get; set; }
        public string? StripePaymentIntentId { get; set; }
        [Required(ErrorMessage = "Please enter name for billing address")]
        [Display(Name = "Customer name")]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your phone number")]
        [RegularExpression(pattern: "/^[0-9]{10}$/", ErrorMessage = "Please enter a valid mobile number of 10 digits")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter street address")]
        [StringLength(maximumLength: 500)]
        public string StreetAddress { get; set; }
        [Required(ErrorMessage = "Please enter city")]
        [StringLength(maximumLength: 100)]
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter state")]
        [StringLength(maximumLength: 100)]
        public string State { get; set; }
        [Required(ErrorMessage = "Please enter postal code")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(pattern: "/^[0-9]{4,6}$/", ErrorMessage = "Please enter a valid pincode of 4 to 6 digits")]
        public string PostalCode { get; set; }
    }
}
