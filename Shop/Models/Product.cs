using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage ="Name must be shorter than 30 characters")]
        public string Name { get; set; }
        [Required]
        [Range(1,20000, ErrorMessage ="Price must be between 1 and 20000")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(15, 2)")]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public ProductType ProductType { get; set; }
        [Display(Name ="Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
    public enum ProductType
    {
        A,B,C,D,other
    }
}
