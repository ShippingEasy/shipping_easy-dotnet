using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Newtonsoft.Json;

namespace ShippingEasy
{
    public class Order
    {
        private readonly List<Recipient> _recipients = new List<Recipient>();

        [JsonProperty("external_order_identifier")]
        public string OrderIdentifier { get; set; }
        [JsonProperty("ordered_at")]
        public DateTimeOffset OrderedAt { get; set; }

        [JsonProperty("recipients")]
        public List<Recipient> Recipients
        {
            get { return _recipients; }
        }
    }

    public class Recipient
    {
        private readonly List<LineItem> _lineItems = new List<LineItem>();
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("address2", NullValueHandling = NullValueHandling.Ignore)]
        public string Address2 { get; set; }
        [JsonProperty("line_items")]
        public List<LineItem> LineItems { get { return _lineItems; } } 
    }

    public class LineItem
    {
        [JsonProperty("item_name")]
        public string ItemName { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}