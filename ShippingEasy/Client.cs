using System;
using Newtonsoft.Json;
using ShippingEasy.Responses;

namespace ShippingEasy
{
    public class Client
    {
        private readonly ResponseHandler _responseHandler;
        public Connection Connection { get; private set; }

        public Client(string apiKey, string apiSecret, string baseUrl) :
            this(new Connection(apiKey, apiSecret, baseUrl)) {}

        public Client(Connection connection)
        {
            Connection = connection;
            _responseHandler = new ResponseHandler();
        }

        /// <summary>
        /// Create a new order in your ShippingEasy account
        /// </summary>
        /// <param name="storeApiKey">The Store API Key that identifies the store where the order will be created.
        /// <remarks>Not to be confused with your account's API Key which is used for authentication.</remarks>
        /// </param>
        /// <param name="order">The details of the order that will be created within the ShippingEasy system.</param>
        /// <returns></returns>
        public CreateOrderResponse CreateOrder(string storeApiKey, Order order)
        {
            var postBody = String.Format("{{\"order\": {0}}}", OrderToJson(order));
            var response = Connection.CreateOrderFromJson(storeApiKey, postBody);
            return _responseHandler.Build<CreateOrderResponse>(response);
        }

        public CancelOrderResponse CancelOrder(string storeApiKey, string externalOrderIdentifier)
        {
            var response = Connection.CancelOrderJson(storeApiKey, externalOrderIdentifier);
            return _responseHandler.Build<CancelOrderResponse>(response);
        }

        /// <summary>
        /// Downloads orders from your ShippingEasy account
        /// </summary>
        /// <param name="query">Optional values used to limit the orders that are returned.</param>
        /// <returns></returns>
        public OrderQueryResponse GetOrders(OrderQuery query = null)
        {
            query = query ?? new OrderQuery();
            var response = query.StoreKey != null
                ? Connection.GetStoreOrdersJson(query.StoreKey, query.ToDictionary())
                : Connection.GetAllOrdersJson(query.ToDictionary());
            return _responseHandler.Build<OrderQueryResponse>(response);
        }

        public static string OrderToJson(Order order)
        {
            return JsonConvert.SerializeObject(order, Formatting.Indented, Serialization.Settings);
        }
    }
}