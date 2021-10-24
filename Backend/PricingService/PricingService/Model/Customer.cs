using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PricingService.Model
{
    public class Customer
    {
        [Required]
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
        public string CustomerName { get; set; }
        public int FreeDays { get; set; }
        //public List<string> CustomerService { get; set; }
    }
}
