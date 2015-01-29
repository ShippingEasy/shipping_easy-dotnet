using Newtonsoft.Json;

namespace ShippingEasy
{
    public class CancelOrderResponse
    {
        public string RawJson { get; private set; }
        [JsonProperty]
        public object Errors { get; private set; }

        public bool Success { get { return Errors == null; } }
        [JsonProperty]
        public Order Order { get; private set; }
    }
}