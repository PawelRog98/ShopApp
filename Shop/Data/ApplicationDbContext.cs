using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Areas.Identity.Data;
using Shop.Models;

namespace Shop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ShopUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Company> Company { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Orders> Orders { get; set; }
    }
}
