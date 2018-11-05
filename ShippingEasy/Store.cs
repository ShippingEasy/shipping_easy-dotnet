using Newtonsoft.Json;

namespace ShippingEasy
{
    public class Store
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
