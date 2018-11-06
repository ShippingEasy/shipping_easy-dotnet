using Newtonsoft.Json;

namespace ShippingEasy
{
    public class OriginalOrder
    {
        public OriginalOrder()
        {

        }

        [JsonProperty("custom_1")]
        public string Custom1 { get; set; }
        [JsonProperty("custom_2")]
        public string Custom2 { get; set; }
        [JsonProperty("custom_3")]
        public string Custom3 { get; set; }
        [JsonProperty("internal_notes")]
        public string InternalNotes { get; set; }
    }
}
