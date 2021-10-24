using PricingService.Model;
using PricingService.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace PricingService.TestService
{
    public class TestPricingService : IPriceingServiceTest
    {
        public float calculatePrice(int customerId, ApplicationDbContext context, string startDate, string endDate)
        {
            BusinessLogicCalculation businessL = new BusinessLogicCalculation();
            var price = businessL.calculatePrice(customerId, context, startDate, endDate);
            return price;
        }

    }
}
