namespace ShippingEasy
{
    public class CreateOrderResponse
    {
        public Order Order { get; set; }
        public string RawJson { get; private set; }
    }
}