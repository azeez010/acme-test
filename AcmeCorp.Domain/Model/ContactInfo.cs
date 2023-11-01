using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Model
{
    public class ContactInfo
    {
        public int ContactInfoId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CustomerId { get; set; } // Foreign key to link contact info to a customer
        public Customer Customer { get; set; } // Navigation property for the customer
    }

    
}
