using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingService.Model;

namespace PricingService.BusinessLogic
{
    public class BusinessLogicCalculation
    {
        private int GetWorkingDays(DateTime from, DateTime to)
        {
            var totalDays = 0;
            for (var date = from; date < to; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday
                    && date.DayOfWeek != DayOfWeek.Sunday)
                    totalDays++;
            }

            return totalDays;
        }
        private int getFreeDays (int customerId, ApplicationDbContext context)
        {
             return context.Customers.FirstOrDefault(u => u.CustomerId == customerId).FreeDays;
        }
        private int getDiscount(int customerId, ApplicationDbContext context)
        {
            return context.Disscounts.FirstOrDefault(u => u.CusteomerId  == customerId).DiscountPrice;
        }
        private DateTime? getDiscountStartDate(int customerId, ApplicationDbContext context)
        {
            DateTime? startTime = context.Disscounts.FirstOrDefault(u => u.CusteomerId == customerId).DiscountStartDate;
            if (startTime != null)
            {
                return startTime;
            }
            return null;
        }
        private DateTime? getDiscountStopDate(int customerId, ApplicationDbContext context)
        {
            DateTime? stopTime = context.Disscounts.FirstOrDefault(u => u.CusteomerId == customerId).DiscountStopDate;
            if (stopTime != null)
            {
                return stopTime;
            }
            return null;
        }
       
        private string getCustomerServices(int customerID, ApplicationDbContext context)
        {
            var services = context.CustomerServices.Where(u => u.CusteomerId == customerID).ToList();
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < services.Count; i++ )
            {
                if (i == services.Count - 1)
                    sb.Append(services[i].Service);
                else
                    sb.Append(services[i].Service + ",");
            }
            return sb.ToString();
        }
        private bool isWorkdaysOnly(string service, ApplicationDbContext context)
        {
            return context.Services.FirstOrDefault(u => u.Name == service).WorkDaysOnly;
                
        }
        private int getDays(string service, ApplicationDbContext context, DateTime from, DateTime to)
        {
            int days = 0;
            if(isWorkdaysOnly(service, context))
            {
                days = GetWorkingDays(from, to);
            }
            else
            {
                days = (int)(to - from).TotalDays;
            }
            return days;
        }
        private float getBasePrice(string service, ApplicationDbContext context)
        {
            return context.Services.FirstOrDefault(u => u.Name == service).BasePrice;
        }
        public int getDiscountDays(int customerId, ApplicationDbContext context, string service, DateTime start, DateTime stop)
        {
            int days = 0;
            if (context.Disscounts.FirstOrDefault(u => u.CusteomerId == customerId).DiscountStartDate == null)
                return 0;
            DateTime startD = (DateTime)getDiscountStartDate(customerId, context);
            DateTime stopD = (DateTime)getDiscountStopDate(customerId, context);
           
            if (start >= startD  && start <= stopD)
            {
                if(stop <= stopD)
                {
                    days = getDays(service, context, start, stop);   //the whole period is discounted.
                }
                else
                {
                    days = getDays(service, context, start, (DateTime)getDiscountStopDate(customerId, context));
                }
            }
            else if (start <= startD && start <= stopD)
            {
                if (stop <= stopD)
                {
                    days = getDays(service, context, startD, stop);  //partial discount days
                }
                else
                {
                    days = getDays(service, context, startD, stopD);
                }

            }
            return days;
        }
        public float calculatePrice(int customerId, ApplicationDbContext context, string startDate, string endDate)
        {
            
            int days = 0;
            float totalPrice = 0;
            int discountDays = 0;
            float discountPrice = 0;
            int nonDiscountDays = 0;
            int freeDays = 0;
    
            DateTime start = DateTime.ParseExact (startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var services = getCustomerServices(customerId, context);
            var serviceArray = services.Trim().Split(",");
            for(int i = 0; i < serviceArray.Length; i++)
            {
                days = getDays(serviceArray[i], context, start, end);
                freeDays = getFreeDays(customerId, context);
                if (days > freeDays)
                {
                    days = days - freeDays;
                }
                else
                {
                    freeDays = freeDays - days;
                    days = 0;
                }
                if (days > 0)
                {
                    var basePrice = getBasePrice(serviceArray[i], context);
                    var discount = getDiscount(customerId, context) / 100F;
                    var discountStartDate = getDiscountStartDate(customerId, context);
                    if (discountStartDate != null && discount > 0)
                    {
                        discountDays = getDiscountDays(customerId, context, serviceArray[i], start, end);
                        nonDiscountDays = days - discountDays;
                        discountPrice = discountDays * discount * basePrice;
                    }
                    else if(discountStartDate == null && discount > 0)
                    {
                        nonDiscountDays = 0;
                        discountPrice = days * discount * basePrice;
                    }

                    totalPrice = totalPrice + (nonDiscountDays * basePrice) + discountPrice;
                }
            }

            return totalPrice;
           
        }

    }
}
