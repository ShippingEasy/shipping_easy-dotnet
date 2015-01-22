namespace ShippingEasy
{
    public class OrderQueryResponse
    {
        private readonly string _responseBody;

        public OrderQueryResponse(string responseBody)
        {
            _responseBody = responseBody;
        }

        public override string ToString()
        {
            return _responseBody;
        }
    }
}