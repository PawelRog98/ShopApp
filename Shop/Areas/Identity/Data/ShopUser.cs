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
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        public string Street { get; set; }
        [PersonalData]
        public string PostalCode { get; set; }
        [PersonalData]
        public string State { get; set; }
        [PersonalData]
        public string Country { get; set; }
    }
}
