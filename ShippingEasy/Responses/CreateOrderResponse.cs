using Newtonsoft.Json;

namespace ShippingEasy.Responses
{
    public class CreateOrderResponse : ApiResponse
    {
        [JsonProperty]
        public Order Order { get; private set; }
    }
}