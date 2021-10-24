using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using PricingService.Model;
using PricingService.TestService; 
using Xunit;


namespace PricingServiceTest
{
    public class PricingServiceCalculationTest
    {
        [Fact]
        public void calculatePriceTest()
        {
            var context = CreateDbContextMock();

            var service = new TestPricingService();

            var result = service.calculatePrice(1, context.Object, "2019-09-20", "2019-10-01");

            // assert
            Assert.Equal(5, result);
        }
        private Mock<ApplicationDbContext> CreateDbContextMock()
        {
            var customers = getCustomers().AsQueryable(); ;
            var discounts = getDiscounts().AsQueryable();
            var specialPrices = getSpecialPrices().AsQueryable();
            var services = getServices().AsQueryable();
            var customerServices = getCustomerServices().AsQueryable();

            var Customers = new Mock<DbSet<Customer>>();
            Customers.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.Provider);
            Customers.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            Customers.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            Customers.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customers.GetEnumerator());

            var Discounts = new Mock<DbSet<Discount>>();
            Discounts.As<IQueryable<Discount>>().Setup(m => m.Provider).Returns(discounts.Provider);
            Discounts.As<IQueryable<Discount>>().Setup(m => m.Expression).Returns(discounts.Expression);
            Discounts.As<IQueryable<Discount>>().Setup(m => m.ElementType).Returns(discounts.ElementType);
            Discounts.As<IQueryable<Discount>>().Setup(m => m.GetEnumerator()).Returns(discounts.GetEnumerator());

            var SpecialPrices = new Mock<DbSet<SpecialPrice>>();
            SpecialPrices.As<IQueryable<SpecialPrice>>().Setup(m => m.Provider).Returns(specialPrices.Provider);
            SpecialPrices.As<IQueryable<SpecialPrice>>().Setup(m => m.Expression).Returns(specialPrices.Expression);
            SpecialPrices.As<IQueryable<SpecialPrice>>().Setup(m => m.ElementType).Returns(specialPrices.ElementType);
            SpecialPrices.As<IQueryable<SpecialPrice>>().Setup(m => m.GetEnumerator()).Returns(specialPrices.GetEnumerator());

            var Services = new Mock<DbSet<Service>>();
            Services.As<IQueryable<Service>>().Setup(m => m.Provider).Returns(services.Provider);
            Services.As<IQueryable<Service>>().Setup(m => m.Expression).Returns(services.Expression);
            Services.As<IQueryable<Service>>().Setup(m => m.ElementType).Returns(services.ElementType);
            Services.As<IQueryable<Service>>().Setup(m => m.GetEnumerator()).Returns(services.GetEnumerator());

            var CustomerServices = new Mock<DbSet<CustomerService>>();
            CustomerServices.As<IQueryable<CustomerService>>().Setup(m => m.Provider).Returns(customerServices.Provider);
            CustomerServices.As<IQueryable<CustomerService>>().Setup(m => m.Expression).Returns(customerServices.Expression);
            CustomerServices.As<IQueryable<CustomerService>>().Setup(m => m.ElementType).Returns(customerServices.ElementType);
            CustomerServices.As<IQueryable<CustomerService>>().Setup(m => m.GetEnumerator()).Returns(customerServices.GetEnumerator());

            var context = new Mock<ApplicationDbContext>();

            context.Setup(c => c.Customers).Returns(Customers.Object);
            context.Setup(c => c.Disscounts).Returns(Discounts.Object);
            context.Setup(c => c.SpecialPrices).Returns(SpecialPrices.Object);
            context.Setup(c => c.Services).Returns(Services.Object);
            context.Setup(c => c.CustomerServices).Returns(CustomerServices.Object);


            AddTestData(context);

            return context;
        }
        private static void AddTestData(Mock<ApplicationDbContext> context)
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
            context.Object.Customers.Add(customer);
            customerServices.Id = 1;
            customerServices.CusteomerId = 1;
            customerServices.Service = "Service A";
            customerServices.ServiceStartDate = null;
            context.Object.CustomerServices.Add(customerServices);
            customerServices2.Id = customerServices.Id + 1;
            customerServices2.CusteomerId = customer.CustomerId;
            customerServices2.Service = "Service C";
            customerServices.ServiceStartDate = null;
            context.Object.CustomerServices.Add(customerServices2);
            context.Object.SaveChanges();

            /*  discount   */
            discount.Id += 1;
            discount.CusteomerId = customer.CustomerId;
            discount.Service = "Service C";
            discount.DiscountStartDate = DateTime.Parse("2019-09-22", CultureInfo.InvariantCulture);
            discount.DiscountStopDate = DateTime.Parse("2019-09-24", CultureInfo.InvariantCulture);
            discount.DiscountPrice = 20;
            context.Object.Disscounts.Add(discount);
            context.Object.SaveChanges();


            /* services */

            var service = new Service();
            service.Id += 1;
            service.Name = "Service A";
            service.BasePrice = 0.20F;
            service.WorkDaysOnly = true;
            context.Object.Services.Add(service);
            var service2 = new Service();
            service2.Id = service.Id + 1;
            service2.Name = "Service B";
            service2.BasePrice = 0.24F;
            service2.WorkDaysOnly = true;
            context.Object.Services.Add(service2);
            var service3 = new Service();
            service3.Id = service2.Id + 1;
            service3.Name = "Service C";
            service3.BasePrice = 0.4F;
            service3.WorkDaysOnly = false;
            context.Object.Services.Add(service3);
            context.Object.SaveChanges();


            /* customer 2 */

            var customer2 = new Customer();

            customer2.Id = 2;
            customer2.CustomerId = 2;
            customer2.CustomerName = "Customer Y";
            customer2.FreeDays = 200;
            context.Object.Customers.Add(customer2);
            discount.Id += 1;
            discount.CusteomerId = customer2.CustomerId;
            discount.DiscountPrice = 30;
            discount.Service = "";
            discount.DiscountStartDate = null;
            discount.DiscountStopDate = null;
            context.Object.Disscounts.Add(discount);
            var customerServices3 = new CustomerService();
            customerServices3.Id = customerServices2.Id + 1;
            customerServices3.CusteomerId = 2;
            customerServices3.Service = "Service B";
            customerServices3.ServiceStartDate = null;
            context.Object.CustomerServices.Add(customerServices3);
            context.Object.SaveChanges();
            var customerServices4 = new CustomerService();
            customerServices4.Id = customerServices3.Id + 1;
            customerServices4.CusteomerId = 2;
            customerServices4.Service = "Service C";
            customerServices4.ServiceStartDate = null;
            context.Object.CustomerServices.Add(customerServices4);

            context.Object.SaveChanges();

        }
        private IEnumerable<Customer> getCustomers()
        {
            IEnumerable<Customer> Customers = A.ListOf<Customer>();
            return Customers;
        }
        private IEnumerable<Discount> getDiscounts()
        {
            IEnumerable<Discount> Discounts = A.ListOf<Discount>();
            return Discounts;
        }
        private IEnumerable<SpecialPrice> getSpecialPrices()
        {
            IEnumerable<SpecialPrice> specialPrices = A.ListOf<SpecialPrice>();
            return specialPrices;
        }
        private IEnumerable<Service> getServices()
        {
            IEnumerable<Service> services = A.ListOf<Service>();
            return services;
        }
        private IEnumerable<CustomerService> getCustomerServices()
        {
            IEnumerable<CustomerService> customerServices = A.ListOf<CustomerService>();
            return customerServices;
        }
    }
}
