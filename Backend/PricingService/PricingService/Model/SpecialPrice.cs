using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Model
{
    public class SpecialPrice
    {
        [Key]
        /*
         * Can not have auto incrument when using in memory db.
         * It should be activated if using a real database.
         * 
         * [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         * 
         * 
         */
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Service { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
    }
}
