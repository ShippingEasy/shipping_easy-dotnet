using System;
using System.Configuration;
using System.Linq;

namespace ShippingEasy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = (args.FirstOrDefault() ?? "ORDERS").ToUpper();
            var appSettings = ConfigurationManager.AppSettings;
            var apiKey = appSettings["ShippingEasy.ApiKey"];
            var apiSecret = appSettings["ShippingEasy.ApiSecret"];
            var baseUrl = appSettings["ShippingEasy.BaseUrl"];
            try
            {
                var client = new Client(apiKey, apiSecret, baseUrl);
                HttpResponse response;
                const string storeApiKey = "c71dc6da574eea04e2c926906bcb4eab";
                switch (command)
                {
                    case "ORDERS":
                        response = client.GetOrders().HttpResponse;
                        break;
                    case "STORE_ORDERS":
                        response = client.GetOrders(new OrderQuery
                        {
                            StoreKey = storeApiKey,
                            Status = "shipped, ready_for_shipment",
                        }).HttpResponse;
                        break;
                    case "CREATE_ORDER":
                        response = client.CreateOrder(storeApiKey, new Order
                        {
                            OrderIdentifier = string.Format("ABC-{0}", DateTime.Now.Ticks),
                            OrderedAt = DateTime.Now,
                            Recipients =
                            {
                                new Recipient
                                {
                                    FirstName = "Colin",
                                    LastName = "Smith",
                                    Address = "1600 Pennsylvania Ave",
                                    Address2 = "Suite # 2",
                                    LineItems = { new LineItem { ItemName = "Sprocket", Quantity = 7 } }
                                }
                            }
                        }).HttpResponse;
                        break;
                    case "CANCEL":
                        var order_id = "ABC-45";
                        response = client.CancelOrder(storeApiKey, order_id).HttpResponse;
                        break;  
                    default:
                        throw new ArgumentException("Unrecognized command: " + command);
                }
                System.Console.WriteLine("Status: {0}\n{1}", response.Status, response.Body);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine("Uncaught exception: {0}", exception);
            }
            
        }
    }
}
