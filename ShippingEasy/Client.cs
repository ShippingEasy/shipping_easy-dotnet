using System;
using Newtonsoft.Json;

namespace ShippingEasy
{
    public class Client
    {
        private readonly Connection _connection;

        public Client()
        {
            _connection = new Connection();
        }

        public Client(string apiKey, string apiSecret, string baseUrl)
        {
            _connection = new Connection(apiKey, apiSecret, baseUrl);
        }

        public Client(Connection connection)
        {
            _connection = connection;
        }

        public CreateOrderResponse CreateOrder(string storeApiKey, Order order)
        {
            var body = String.Format("{{\"order\": {0}}}", OrderToJson(order));
            var responseBody = _connection.CreateOrderFromJson(storeApiKey, body);
            return new CreateOrderResponse(responseBody);
        }

        public OrderQueryResponse GetOrders(OrderQuery query)
        {
            var responseBody = query.StoreKey != null ?
                _connection.GetStoreOrdersJson(query.StoreKey, query.ToDictionary()) :
                _connection.GetAllOrdersJson(query.ToDictionary());
            return new OrderQueryResponse(responseBody);
        }

        public Order ParseOrder(string json)
        {
            return JsonConvert.DeserializeObject<Order>(json);
        }

        public string OrderToJson(Order order)
        {
            return JsonConvert.SerializeObject(order, Formatting.Indented);
        }
    }
}