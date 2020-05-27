using Newtonsoft.Json;
using System;

namespace ShippingEasy
{
    public class Shipment
    {
        public string ID { get; set; }
        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }
        [JsonProperty("carrier_key")]
        public string CarrierKey { get; set; }
        [JsonProperty("carrier_service_key")]
        public string CarrierServiceKey { get; set; }
        [JsonProperty("shipment_cost")]
        public decimal? ShipmentCost { get; set; }
        [JsonProperty("ship_date")]
        public DateTime? ShipDate { get; set; }
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        [JsonProperty("cloned_from_shipment_id")]
        public string ClonedFromShipmentID { get; set; }
        [JsonProperty("weight_in_ounces")]
        public decimal? WeightInOunches { get; set; }
        [JsonProperty("length_in_ounces")]
        public decimal? LengthInInches { get; set; }
        [JsonProperty("width_in_ounces")]
        public decimal? WidthInInches { get; set; }
        [JsonProperty("height_in_ounces")]
        public decimal? HeightInInches { get; set; }

    }
}
