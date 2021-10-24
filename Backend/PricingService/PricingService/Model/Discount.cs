using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Model
{
    public class Discount
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
        public int CusteomerId { get; set; }
        public string Service { get; set; }
        public int DiscountPrice { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountStopDate { get; set; }
    }
}
