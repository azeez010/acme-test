using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } 
        public decimal Amount { get; set; }
        public bool Status { get; set; }  //Indicate if an order has been paid for or not
        public int CustomerId { get; set; } // Foreign key to link the order to a customer
        public Customer Customer { get; set; } // Navigation property for the customer
    }

    

}
