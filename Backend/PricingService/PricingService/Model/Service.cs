using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Model
{
    public class Service
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
        public string Name{ get; set; }
        public float BasePrice { get; set; }
        public bool WorkDaysOnly { get; set; }
    }
    
}
