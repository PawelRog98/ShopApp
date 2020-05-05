using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public int productId { get; set; }
        [ForeignKey("productId")]
        public Product product { get; set; }
        public int CartQuantity { get; set; }
        public string UserName { get; set; }
        public int ordersId{ get; set; }
        [ForeignKey("ordersId")]
        public Orders orders { get; set; }
    }
}
