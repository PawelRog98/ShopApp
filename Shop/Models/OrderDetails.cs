using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public int OrdersId { get; set; }
        [ForeignKey("OrdersId")]
        public Orders Orders { get; set; }
    }
}
