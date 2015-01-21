using System;
using System.Collections.Generic;
using System.Linq;

namespace ShippingEasy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = (args.FirstOrDefault() ?? "ORDERS").ToUpper();
            try
            {
                var connection = new Connection();
                var client = new Client(connection);
                string response;
                switch (command)
                {
                    case "ORDERS":
                        response = connection.GetAllOrdersJson(new Dictionary<string, string>
                        {
                            {"status", "ready_for_shipment"},
                            {"page", "2"}, {"per_page", "2"},
                            {"last_updated_at", "2015-01-20"}
                        });
                        break;
                    case "STORE_ORDERS":
                        response = connection.GetStoreOrdersJson("c71dc6da574eea04e2c926906bcb4eab",
                            new Dictionary<string, string>{{"status", "ready_for_shipment"}});
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
                        });
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
