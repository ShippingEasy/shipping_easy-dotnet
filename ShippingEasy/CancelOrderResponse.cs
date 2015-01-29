using Newtonsoft.Json;

namespace ShippingEasy
{
    public class CancelOrderResponse : ApiResponse
    {
        [JsonProperty]
        public Order Order { get; private set; }
    }
}