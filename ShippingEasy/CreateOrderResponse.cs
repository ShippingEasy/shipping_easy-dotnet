namespace ShippingEasy
{
    public class CreateOrderResponse
    {
        private readonly string _responseBody;

        public CreateOrderResponse(string responseBody)
        {
            _responseBody = responseBody;
        }

        public override string ToString()
        {
            return _responseBody;
        }
    }
}