using System.Collections.Generic;

namespace ShippingEasy
{
    public class OrderQueryResponse
    {
        public IList<Order> Orders { get; set; }
        public string RawJson { get; private set; }
    }
}