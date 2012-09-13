using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobinHood.Models
{
    public class CreditCard
    {
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
    }
}
