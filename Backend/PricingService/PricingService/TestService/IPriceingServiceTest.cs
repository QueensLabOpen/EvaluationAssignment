using PricingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.TestService
{
    public interface IPriceingServiceTest
    {
        float calculatePrice(int customerId, ApplicationDbContext context, string startDate, string endDate);
    }
}
