using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }
        public string OrderName { get; set; }
        public string UserName { get; set; }
        public DateTime dateTime { get; set; }
        public decimal TransportCost { get; set; }
    }
}
