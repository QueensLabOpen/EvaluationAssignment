using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PricingService.Model;
using PricingService.BusinessLogic;
using Microsoft.AspNetCore.Http;

namespace PricingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PricingServiceController : ControllerBase
    {
       
        private readonly ApplicationDbContext context;

        public PricingServiceController(ApplicationDbContext db)
        {
            context = db;
        }

        [HttpGet("{id}/{fromDate}/{toDate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public float Get(int id, string fromDate, string toDate)
        {
            BusinessLogicCalculation businessL = new BusinessLogicCalculation();
            var price =  businessL.calculatePrice(id, context,  fromDate, toDate);
            return price;
        }
    }
}
