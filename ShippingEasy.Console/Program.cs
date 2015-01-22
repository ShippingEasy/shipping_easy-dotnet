using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ShippingEasy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = (args.FirstOrDefault() ?? "ORDERS").ToUpper();
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            var apiKey = appSettings["ShippingEasy.ApiKey"];
            var apiSecret = appSettings["ShippingEasy.ApiSecret"];
            var baseUrl = appSettings["ShippingEasy.BaseUrl"];
            try
            {
                var client = new Client(apiKey, apiSecret, baseUrl);
                string response;
                switch (command)
                {
                    case "ORDERS":
                        response =
                            client.GetOrders(new OrderQuery
                            {
                                Status = "ready_for_shipment",
                                Page = 2,
                                ResultsPerPage = 2
                            }).ToString();
                        break;
                    case "STORE_ORDERS":
                        response = client.GetOrders(new OrderQuery
                        {
                            StoreKey = "c71dc6da574eea04e2c926906bcb4eab",
                            Status = "shipped, ready_for_shipment",
                        }).ToString();
                        break;
                    case "CREATE_ORDER":
                        response = client.CreateOrder("c71dc6da574eea04e2c926906bcb4eab", new Order
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
                        }).ToString();
                        break;
                    default:
                        throw new ArgumentException("Unrecognized command: " + command);
                }
                System.Console.WriteLine(response);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }
            
        }
    }
}
