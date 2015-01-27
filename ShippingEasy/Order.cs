using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShippingEasy
{
    public class Order
    {
        private readonly List<Recipient> _recipients = new List<Recipient>();
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? SystemId { get; private set; }
        [JsonProperty("external_order_identifier")]
        public string OrderIdentifier { get; set; }
        public DateTimeOffset? OrderedAt { get; set; }
        /// <summary>
        /// Time order was last updated by ShippingEasy.
        /// </summary>
        [JsonProperty]
        public DateTimeOffset? UpdatedAt { get; private set; }

        public List<Recipient> Recipients
        {
            get { return _recipients; }
        }

        [JsonProperty("order_status")]
        public string Status { get; set; }

        public decimal? TotalIncludingTax { get; set; }
        public decimal? TotalExcludingTax { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? CouponDiscount { get; set; }
        public decimal? SubtotalIncludingTax { get; set; }
        public decimal? SubtotalExcludingTax { get; set; }
        public decimal? SubtotalTax { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? BaseShippingCost { get; set; }
        public decimal? ShippingCostIncludingTax { get; set; }
        public decimal? ShippingCostExcludingTax { get; set; }
        public decimal? ShippingCostTax { get; set; }
        public decimal? BaseHandlingCost { get; set; }
        public decimal? HandlingCostExcludingTax { get; set; }
        public decimal? HandlingCostIncludingTax { get; set; }
        public decimal? HandlingCostTax { get; set; }
        public decimal? BaseWrappingCost { get; set; }
        public decimal? WrappingCostExcludingTax { get; set; }
        public decimal? WrappingCostIncludingTax { get; set; }
        public decimal? WrappingCostTax { get; set; }
        public string BillingCompany { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingAddress { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingCountry { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingPhoneNumber { get; set; }
        public string BillingEmail { get; set; }
        [JsonProperty]
        public string StoreApiKey { get; private set; }
    }

    public class Recipient
    {
        private readonly List<LineItem> _lineItems = new List<LineItem>();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public List<LineItem> LineItems { get { return _lineItems; } }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool? Residential { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        [JsonProperty("postal_code_plus_4")]
        public string PostalCodePlus4 { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal? BaseCost { get; set; }
        public decimal? CostExcludingTax { get; set; }
        public decimal? CostIncludingTax { get; set; }
        public decimal? CostTax { get; set; }
        public decimal? BaseHandlingCost { get; set; }
        public decimal? HandlingCostIncludingTax { get; set; }
        public decimal? HandlingCostExcludingTax { get; set; }
        public decimal? HandlingCostTax { get; set; }
        public int? ShippingZoneId { get; set; }
        public string ShippingZoneName { get; set; }
        public int? ItemsTotal { get; set; }
        public string ShippingMethod { get; set; }
        public int? ItemsShipped { get; set; }
        public int? ExtShippingDetailId { get; set; }
    }

    public class LineItem
    {
        private IDictionary<string, string> _productOptions = new Dictionary<string, string>();
        public string ItemName { get; set; }
        public int? Quantity { get; set; }
        public string Sku { get; set; }
        public string BinPickingNumber { get; set; }
        public decimal? WeightInOunces { get; set; }
        public decimal? TotalExcludingTax { get; set; }
        public decimal? PriceExcludingTax { get; set; }
        public decimal? UnitPrice { get; set; }
        public string ExtLineItemId { get; set; }
        public string ExtProductId { get; set; }

        public IDictionary<string, string> ProductOptions
        {
            get { return _productOptions; }
        }
    }
}