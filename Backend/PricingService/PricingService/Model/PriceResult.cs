using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Model
{
    public class PriceResult
    {
        public string CustomerName { get; set; }
        public string ServicesUse { get; set; }
        public string DatesOfService { get; set; }
        public double TotalPrice { get; set; }
    }
}
