using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Areas.Identity.Data
{
    public class ShopUser:IdentityUser
    {
        [PersonalData]
        [Required]
        public string FirstName { get; set; }
        [Required]
        [PersonalData]
        public string LastName { get; set; }
        [Required]
        [PersonalData]
        public string City { get; set; }
        [Required]
        [PersonalData]
        public string Street { get; set; }
        [Required]
        [PersonalData]
        public string PostalCode { get; set; }
        [Required]
        [PersonalData]
        public string State { get; set; }
        [Required]
        [PersonalData]
        public string Country { get; set; }
    }
}
