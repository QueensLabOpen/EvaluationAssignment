using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PricingService.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
       

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Discount> Disscounts { get; set; }
        public DbSet<SpecialPrice> SpecialPrices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<CustomerService> CustomerServices { get; set; }



    }
}
