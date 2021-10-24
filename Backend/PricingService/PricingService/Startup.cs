using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PricingService.Model;
using System.Globalization;
using PricingService.TestService;

namespace PricingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PricingService", Version = "v1" });
            });
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "QueenDatabase"), ServiceLifetime.Scoped, ServiceLifetime.Scoped);
            services.AddTransient<IPriceingServiceTest, TestPricingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PricingService v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var localScope = app.ApplicationServices.CreateScope();
            var context = localScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            AddTestData(context);
        }
        /*
         * Generate test data at startup. Fill the in memory database.
         * 
         * 
         */
        private static void AddTestData(ApplicationDbContext context)
        {
            
            var discount = new Discount();
            var customerServices = new CustomerService();
            var customerServices2 = new CustomerService();
            var spcialPrice = new SpecialPrice();
            var customer = new Customer();

            /*  Customer 1 */
            customer.Id = 1;
            customer.CustomerId = 1;
            customer.CustomerName = "Customer X";
            customer.FreeDays = 0;
            context.Customers.Add(customer);
            customerServices.Id = 1;
            customerServices.CusteomerId = 1;
            customerServices.Service = "Service A";
            customerServices.ServiceStartDate = null;
            context.CustomerServices.Add(customerServices);
            customerServices2.Id = customerServices.Id + 1;
            customerServices2.CusteomerId = customer.CustomerId;
            customerServices2.Service = "Service C";
            customerServices.ServiceStartDate = null;
            context.CustomerServices.Add(customerServices2);
            context.SaveChanges();

            /*  discount   */
            discount.Id += 1;
            discount.CusteomerId = customer.CustomerId;
            discount.Service = "Service C";
            discount.DiscountStartDate = DateTime.Parse("2019-09-22", CultureInfo.InvariantCulture);
            discount.DiscountStopDate = DateTime.Parse("2019-09-24", CultureInfo.InvariantCulture);
            discount.DiscountPrice = 20;
            context.Disscounts.Add(discount);
            context.SaveChanges();
            
            
            /* services */

            var service = new Service();
            service.Id += 1;
            service.Name = "Service A";
            service.BasePrice = 0.20F;
            service.WorkDaysOnly = true;
            context.Services.Add(service);
            var service2 = new Service();
            service2.Id = service.Id + 1;
            service2.Name = "Service B";
            service2.BasePrice = 0.24F;
            service2.WorkDaysOnly = true;
            context.Services.Add(service2);
            var service3 = new Service();
            service3.Id = service2.Id + 1;
            service3.Name = "Service C";
            service3.BasePrice = 0.4F;
            service3.WorkDaysOnly = false;
            context.Services.Add(service3);
            context.SaveChanges();


            /* customer 2 */

            var customer2 = new Customer();

            customer2.Id = 2;
            customer2.CustomerId = 2;
            customer2.CustomerName = "Customer Y";
            customer2.FreeDays = 200;
            context.Customers.Add(customer2);
            discount.Id += 1;
            discount.CusteomerId = customer2.CustomerId;
            discount.DiscountPrice = 30;
            discount.Service = "";
            discount.DiscountStartDate = null;
            discount.DiscountStopDate = null;
            context.Disscounts.Add(discount);
            var customerServices3 = new CustomerService();
            customerServices3.Id = customerServices2.Id + 1;
            customerServices3.CusteomerId = 2;
            customerServices3.Service = "Service B";
            customerServices3.ServiceStartDate = null;
            context.CustomerServices.Add(customerServices3);
            context.SaveChanges();
            var customerServices4 = new CustomerService();
            customerServices4.Id = customerServices3.Id + 1;
            customerServices4.CusteomerId = 2;
            customerServices4.Service = "Service C";
            customerServices4.ServiceStartDate = null;
            context.CustomerServices.Add(customerServices4);

            context.SaveChanges();

        }

    }
}
