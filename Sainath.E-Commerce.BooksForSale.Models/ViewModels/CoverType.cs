using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sainath.E_Commerce.BooksForSale.Models.ViewModels
{
    [Table(name: "CoverTypes")]
    public class CoverType
    {
        [Key]
        [Column(name: "Id")]
        public int CoverTypeId { get; set; }
        [Required(ErrorMessage = "Please enter cover type name")]
        [MaxLength(length: 50)]
        [Column(name: "Name")]
        [Display(Name = "Cover type name")]
        public string CoverTypeName { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
    }
}
