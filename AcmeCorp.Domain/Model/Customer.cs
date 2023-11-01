using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public List<ContactInfo> ContactInfo { get; set; } // A customer can have multiple contact info entries
        public List<Order> Orders { get; set; } // A customer can have multiple orders
    }


}
