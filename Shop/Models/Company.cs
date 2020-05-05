using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }
        public Country Country { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
    public enum Country
    {
        Poland, France, Germany, USA, China, Italy, Other
    }
}
