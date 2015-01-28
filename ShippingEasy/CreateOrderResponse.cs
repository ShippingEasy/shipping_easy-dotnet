using Newtonsoft.Json;

namespace ShippingEasy
{
    public class CreateOrderResponse
    {
        [JsonProperty]
        public Order Order { get; private set; }
        public string RawJson { get; private set; }
        [JsonProperty]
        public object Errors { get; private set; }

        public bool Success { get { return Errors == null; } }
    }
}