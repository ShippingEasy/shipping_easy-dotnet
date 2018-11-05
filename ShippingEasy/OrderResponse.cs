using ShippingEasy.Responses;

namespace ShippingEasy
{
    public class OrderResponse : ApiResponse
    {
        private readonly Order _order = new Order();

        public Order Order
        {
            get { return _order; }
        }
    }
}
