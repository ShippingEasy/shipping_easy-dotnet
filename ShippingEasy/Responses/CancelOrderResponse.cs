using Newtonsoft.Json;

namespace ShippingEasy.Responses
{
    public class CancelOrderResponse : ApiResponse
    {
        [JsonProperty]
        public Order Order { get; private set; }
    }
}