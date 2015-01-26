using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShippingEasy
{
    public class Order
    {
        private readonly List<Recipient> _recipients = new List<Recipient>();
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? SystemId { get; set; }
        [JsonProperty("external_order_identifier")]
        public string OrderIdentifier { get; set; }
        public DateTimeOffset? OrderedAt { get; set; }

        public List<Recipient> Recipients
        {
            get { return _recipients; }
        }

        [JsonProperty("order_status")]
        public string Status { get; set; }

        public decimal? TotalIncludingTax { get; set; }
    }

    public class Recipient
    {
        private readonly List<LineItem> _lineItems = new List<LineItem>();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public List<LineItem> LineItems { get { return _lineItems; } } 
    }

    public class LineItem
    {
        public string ItemName { get; set; }
        public int? Quantity { get; set; }
    }
}