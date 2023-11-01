using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.DTO
{
    public class AddContactInfoDto
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CustomerId { get; set; }
    }

}
