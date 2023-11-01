using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.DTO
{
    public class CreateOrderDto
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public bool Status { get; set; } = false;
        public int CustomerId { get; set; }
    }
}
