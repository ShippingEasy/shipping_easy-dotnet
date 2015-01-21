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

        public string CreateOrder(string storeApiKey, Order order)
        {
            var body = String.Format("{{\"order\": {0}}}", OrderToJson(order));
            return _connection.CreateOrderFromJson(storeApiKey, body);
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