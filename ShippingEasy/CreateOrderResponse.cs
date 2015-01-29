using Newtonsoft.Json;

namespace ShippingEasy
{
    public class CreateOrderResponse : ApiResponse
    {
        [JsonProperty]
        public Order Order { get; private set; }
    }
}